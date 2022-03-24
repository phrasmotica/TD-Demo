using System.Linq;
using UnityEngine;
using TDDemo.Assets.Scripts.Util;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;

namespace TDDemo.Assets.Scripts.Towers
{
    public class ShootProjectile : BaseBehaviour
    {
        /// <summary>
        /// The projectile prefab.
        /// </summary>
        public GameObject ProjectilePrefab;

        /// <summary>
        /// The initial velocity of shot projectiles.
        /// </summary>
        [Range(1, 10)]
        public int ProjectileSpeed;

        /// <summary>
        /// The rate of fire in shots per second.
        /// </summary>
        [Range(1, 10)]
        public int FireRate;

        /// <summary>
        /// The range of the projectile.
        /// </summary>
        [Range(1, 50)]
        public int Range;

        /// <summary>
        /// The tower.
        /// </summary>
        private TowerBehaviour _tower;

        /// <summary>
        /// The time in seconds since the last shot was fired, or null if the tower has not fired a
        /// shot yet. Tracking this ensures a tower can start firing immediately whenever an enemy
        /// appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? _timeSinceLastShot;

        /// <summary>
        /// Get reference to tower script.
        /// </summary>
        private void Start()
        {
            // upgrade objects are children of the original tower
            _tower = GetComponent<TowerBehaviour>();

            if (_tower == null)
            {
                _tower = GetComponentInParent<TowerBehaviour>();
            }

            logger = new MethodLogger(nameof(ShootProjectile));
        }

        /// <summary>
        /// Update time since last shot and check for enemies.
        /// </summary>
        private void Update()
        {
            if (!_timeSinceLastShot.HasValue)
            {
                _timeSinceLastShot = Time.deltaTime;
            }
            else
            {
                _timeSinceLastShot += Time.deltaTime;
            }

            CheckForEnemies();
        }

        /// <summary>
        /// Checks for enemies in this tower's range.
        /// </summary>
        private void CheckForEnemies()
        {
            if (_tower.IsFiring())
            {
                var enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy)
                                        .Where(e => transform.GetDistanceToObject(e) <= Range)
                                        .OrderBy(e => transform.GetDistanceToObject(e))
                                        .ToArray();

                if (enemies.Any())
                {
                    var nearestEnemy = enemies[0];
                    var distance = transform.GetDistanceToObject(nearestEnemy);

                    // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                    if (!_timeSinceLastShot.HasValue || _timeSinceLastShot >= 1f / FireRate)
                    {
                        _timeSinceLastShot = 0;
                        Shoot(nearestEnemy.GetComponent<Enemy>());
                    }
                }
            }
        }

        /// <summary>
        /// Shoots a projectile at an enemy.
        /// </summary>
        private void Shoot(Enemy enemy)
        {
            logger.Log($"Shoot {enemy.gameObject.name}, position {enemy.transform.position}");

            var projectile = Instantiate(ProjectilePrefab, gameObject.transform);

            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.StartPosition = transform.position;
            projectileComponent.Range = Range;

            var rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirectionToTransform(enemy.transform) * ProjectileSpeed;
        }

        /// <summary>
        /// Returns a normalised vector pointing to another transform component.
        /// </summary>
        private Vector2 GetDirectionToTransform(Transform t)
        {
            return -(transform.position - t.position).normalized;
        }
    }
}

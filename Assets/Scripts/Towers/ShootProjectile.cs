using System.Linq;
using UnityEngine;
using Assets.Scripts.Extensions;
using Assets.Scripts.Enemies;
using Assets.Scripts.Util;

namespace Assets.Scripts.Towers
{
    public class ShootProjectile : MonoBehaviour
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
        private Tower tower;

        /// <summary>
        /// The time in seconds since the last shot was fired, or null if the tower has not fired a
        /// shot yet. Tracking this ensures a tower can start firing immediately whenever an enemy
        /// appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? timeSinceLastShot;

        /// <summary>
        /// Get reference to tower script.
        /// </summary>
        private void Start()
        {
            // upgrade objects are children of the original tower
            tower = GetComponent<Tower>() ?? GetComponentInParent<Tower>();
        }

        /// <summary>
        /// Update time since last shot and check for enemies.
        /// </summary>
        private void Update()
        {
            if (!timeSinceLastShot.HasValue)
            {
                timeSinceLastShot = Time.deltaTime;
            }
            else
            {
                timeSinceLastShot += Time.deltaTime;
            }

            CheckForEnemies();
        }

        /// <summary>
        /// Checks for enemies in this tower's range.
        /// </summary>
        private void CheckForEnemies()
        {
            if (tower.CanFire)
            {
                var enemies = GameObject.FindGameObjectsWithTag(Tags.EnemyTag)
                                        .Where(e => transform.GetDistanceToObject(e) <= Range)
                                        .OrderBy(e => transform.GetDistanceToObject(e))
                                        .ToArray();

                if (enemies.Any())
                {
                    var nearestEnemy = enemies[0];
                    var distance = transform.GetDistanceToObject(nearestEnemy);

                    // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                    if (!timeSinceLastShot.HasValue || timeSinceLastShot >= 1f / FireRate)
                    {
                        timeSinceLastShot = 0;
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
            Debug.Log($"Shoot {enemy.gameObject.name}, position {enemy.transform.position}");

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

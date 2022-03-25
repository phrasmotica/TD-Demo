using System.Linq;
using UnityEngine;
using TDDemo.Assets.Scripts.Util;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;

namespace TDDemo.Assets.Scripts.Towers
{
    public class ShootProjectile : BaseBehaviour
    {
        public TowerLevel Level { get; set; }

        public bool CanFire { get; set; }

        /// <summary>
        /// The time in seconds since the last shot was fired, or null if the tower has not fired a
        /// shot yet. Tracking this ensures a tower can start firing immediately whenever an enemy
        /// appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? _timeSinceLastShot;

        private AudioSource _audio;

        /// <summary>
        /// Get reference to tower script.
        /// </summary>
        private void Start()
        {
            _audio = GetComponent<AudioSource>();

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
            if (CanFire)
            {
                // TODO: avoid having to compute this in every Update
                var enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy)
                                        .Where(e => transform.GetDistanceToObject(e) <= Level.Range)
                                        .OrderBy(e => transform.GetDistanceToObject(e))
                                        .ToArray();

                if (enemies.Any())
                {
                    var nearestEnemy = enemies[0];
                    var distance = transform.GetDistanceToObject(nearestEnemy);

                    // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                    if (!_timeSinceLastShot.HasValue || _timeSinceLastShot >= 1f / Level.FireRate)
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

            var projectileObj = Instantiate(Level.ProjectilePrefab, gameObject.transform);

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.StartPosition = transform.position;
            projectile.Damage = Level.Damage;
            projectile.Range = Level.Range;

            var rb = projectileObj.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirectionToTransform(enemy.transform) * Level.ProjectileSpeed;

            _audio.Play();
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

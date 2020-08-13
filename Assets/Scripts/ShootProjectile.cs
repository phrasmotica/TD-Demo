using System.Linq;
using UnityEngine;
using Assets.Scripts.Extensions;
using Assets.Scripts.Towers;

namespace Assets.Scripts
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
        /// Start checking for enemies.
        /// </summary>
        void Start()
        {
            tower = gameObject.GetComponent<Tower>();

            InvokeRepeating(nameof(CheckForEnemies), 0, 1f / FireRate);
        }

        /// <summary>
        /// Checks for enemies in this tower's range.
        /// </summary>
        private void CheckForEnemies()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy")
                                    .Where(e => transform.GetDistanceToObject(e) <= Range)
                                    .OrderBy(e => transform.GetDistanceToObject(e))
                                    .ToArray();

            if (enemies.Any())
            {
                var nearestEnemy = enemies[0];
                var distance = transform.GetDistanceToObject(nearestEnemy);
                Debug.Log($"Distance: {distance}");

                Shoot(nearestEnemy.GetComponent<Enemy>());
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

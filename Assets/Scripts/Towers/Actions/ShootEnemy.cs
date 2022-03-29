using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ShootEnemy : BaseBehaviour, ITowerAction, IHasDamage, IHasFireRate, IHasRange
    {
        public ProjectileSpecs Specs;

        /// <summary>
        /// The time in seconds since the last shot was fired, or null if the tower has not fired a
        /// shot yet. Tracking this ensures a tower can start firing immediately whenever an enemy
        /// appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? _timeSinceLastShot;

        private AudioSource _audio;

        public int Damage => Specs.Damage;

        public float FireRate => Specs.FireRate;

        public int Range => Specs.Range;

        public bool CanAct { get; set; }

        private void Start()
        {
            _audio = GetComponent<AudioSource>();

            logger = new MethodLogger(nameof(ShootEnemy));
        }

        public void Act(IEnumerable<GameObject> enemies)
        {
            if (!_timeSinceLastShot.HasValue)
            {
                _timeSinceLastShot = Time.deltaTime;
            }
            else
            {
                _timeSinceLastShot += Time.deltaTime;
            }

            if (CanAct)
            {
                CheckForEnemiesInRange(enemies);
            }
        }

        /// <summary>
        /// Checks for enemies in this tower's range.
        /// </summary>
        private void CheckForEnemiesInRange(IEnumerable<GameObject> enemies)
        {
            var inRangeEnemies = enemies.Where(e => transform.GetDistanceToObject(e) <= Range);
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, Specs.TargetMethod);

            if (orderedEnemies.Any())
            {
                var nearestEnemy = orderedEnemies.First();

                // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                if (!_timeSinceLastShot.HasValue || _timeSinceLastShot >= 1f / Specs.FireRate)
                {
                    _timeSinceLastShot = 0;
                    Shoot(nearestEnemy.GetComponent<Enemy>());
                }
            }
        }

        /// <summary>
        /// Shoots a projectile at an enemy.
        /// </summary>
        private void Shoot(Enemy enemy)
        {
            logger.Log($"Shoot {enemy.gameObject.name}, position {enemy.transform.position}");

            var projectileObj = Instantiate(Specs.ProjectilePrefab, gameObject.transform);

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.StartPosition = transform.position;
            projectile.Damage = Specs.Damage;
            projectile.Range = Specs.Range;

            var rb = projectileObj.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirectionToTransform(enemy.transform) * Specs.ProjectileSpeed;

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

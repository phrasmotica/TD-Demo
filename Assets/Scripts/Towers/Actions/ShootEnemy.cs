using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ShootEnemy : BaseBehaviour, ITowerAction, IHasFireRate, IHasRange
    {
        public StrikeProvider StrikeProvider;

        public ProjectileSpecs Specs;

        private TimeCounter _lastShotCounter;

        private AudioSource _audio;

        public float FireRate => Specs.FireRate;

        public int Range => Specs.Range;

        public bool CanAct { get; set; }

        private void Start()
        {
            _lastShotCounter = new(1f / FireRate);
            _lastShotCounter.Start();

            _audio = GetComponent<AudioSource>();

            logger = new MethodLogger(nameof(ShootEnemy));
        }

        public void Act(IEnumerable<GameObject> enemies)
        {
            _lastShotCounter.Increment(Time.deltaTime);

            if (CanAct)
            {
                CheckForEnemiesInRange(enemies);
            }
        }

        private void CheckForEnemiesInRange(IEnumerable<GameObject> enemies)
        {
            var inRangeEnemies = enemies.Where(e => transform.GetDistanceToObject(e) <= Range);
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, Specs.TargetMethod);

            if (orderedEnemies.Any())
            {
                var nearestEnemy = orderedEnemies.First();

                // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                if (!_lastShotCounter.IsRunning || _lastShotCounter.IsFinished)
                {
                    _lastShotCounter.Reset();
                    Shoot(nearestEnemy.GetComponent<Enemy>());
                }
            }
        }

        private void Shoot(Enemy enemy)
        {
            logger.Log($"Shoot {enemy.gameObject.name}, position {enemy.transform.position}");

            var projectileObj = Instantiate(Specs.ProjectilePrefab, gameObject.transform);

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.StrikeProvider = StrikeProvider;
            projectile.StartPosition = transform.position;
            projectile.Range = Specs.Range;

            var rb = projectileObj.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirectionToTransform(enemy.transform) * Specs.ProjectileSpeed;

            if (_audio != null)
            {
                _audio.Play();
            }
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

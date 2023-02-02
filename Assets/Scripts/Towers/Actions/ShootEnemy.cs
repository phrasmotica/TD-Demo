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

        public GameObject ProjectilePrefab;

        [Range(1, 10)]
        public int ProjectileSpeed;

        [Range(1, 10)]
        public int FireRate;

        [Range(1, 10)]
        public int Range;

        public LineRenderer TargetLine;

        public bool ShowTargetLine;

        private TimeCounter _lastShotCounter;

        private AudioSource _audio;

        public TargetMethod TargetMethod { get; set; }

        public bool CanAct { get; set; }

        private void Start()
        {
            if (TargetLine != null)
            {
                TargetLine.enabled = false;
            }

            _lastShotCounter = new(1f / FireRate);
            _lastShotCounter.Start();

            _audio = GetComponent<AudioSource>();

            logger = new MethodLogger(nameof(ShootEnemy));
        }

        public float GetFireRate() => FireRate;

        public int GetRange() => Range;

        public void Ready()
        {
            // need to execute this AFTER the parent tower has been placed
            // to get the correct position for the start of the line
            if (TargetLine != null)
            {
                TargetLine.SetPosition(0, transform.position);
            }
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
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, TargetMethod);

            if (orderedEnemies.Any())
            {
                var target = orderedEnemies.First();

                if (ShowTargetLine && TargetLine != null)
                {
                    TargetLine.enabled = true;
                    TargetLine.SetPosition(1, target.transform.position);
                }

                // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                if (!_lastShotCounter.IsRunning || _lastShotCounter.IsFinished)
                {
                    _lastShotCounter.Restart();
                    Shoot(target.GetComponent<Enemy>());
                }
            }
            else
            {
                TargetLine.enabled = false;
            }
        }

        private void Shoot(Enemy enemy)
        {
            logger.Log($"Shoot {enemy.gameObject.name}, position {enemy.transform.position}");

            var projectileObj = Instantiate(ProjectilePrefab, gameObject.transform);

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.StrikeProvider = StrikeProvider;
            projectile.StartPosition = transform.position;
            projectile.Range = Range;

            var rb = projectileObj.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirectionToTransform(enemy.transform) * ProjectileSpeed;

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

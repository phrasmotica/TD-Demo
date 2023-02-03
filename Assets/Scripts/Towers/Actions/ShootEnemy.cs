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
        public TowerBehaviour SourceTower;

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

        private Enemy _target;

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
            if (TargetLine != null)
            {
                TargetLine.SetPosition(0, transform.position);
            }
        }

        public void Survey(IEnumerable<GameObject> enemies)
        {
            _target = EstablishTarget(enemies);

            var isTargeting = _target != null && SourceTower.IsFiring();
            var shouldDraw = ShowTargetLine && TargetLine != null;

            if (isTargeting && shouldDraw)
            {
                TargetLine.enabled = true;
                TargetLine.SetPosition(1, _target.transform.position);
            }
            else
            {
                TargetLine.enabled = false;
            }
        }

        public void Act()
        {
            if (CanAct)
            {
                _lastShotCounter.Increment(Time.deltaTime);

                FireAtTarget();
            }
        }

        private Enemy EstablishTarget(IEnumerable<GameObject> enemies)
        {
            var inRangeEnemies = enemies.Where(e => transform.GetDistanceToObject(e) <= Range);
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, TargetMethod);

            if (orderedEnemies.Any())
            {
                return orderedEnemies.First().GetComponent<Enemy>();
            }

            return null;
        }

        private void FireAtTarget()
        {
            // if there is an enemy in range and enough time has passed since the last shot, fire a shot
            if (_target != null && !_lastShotCounter.IsRunning || _lastShotCounter.IsFinished)
            {
                _lastShotCounter.Restart();
                Shoot();
            }
        }

        private void Shoot()
        {
            logger.Log($"Shoot {_target.gameObject.name}, position {_target.transform.position}");

            var projectileObj = Instantiate(ProjectilePrefab, gameObject.transform);

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.StrikeProvider = StrikeProvider;
            projectile.StartPosition = transform.position;
            projectile.Range = Range;

            var rb = projectileObj.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirectionToTransform(_target.transform) * ProjectileSpeed;

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

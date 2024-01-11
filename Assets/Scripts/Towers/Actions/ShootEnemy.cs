using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ShootEnemy : BaseBehaviour, ITowerAction, IHasFireRate, IHasRange
    {
        public TowerBehaviour SourceTower;

        public StrikeProvider StrikeProvider;

        public GameObject ProjectilePrefab;

        public GameObject ExplosionPrefab;

        [Range(1, 10)]
        public int ProjectileSpeed;

        [Range(0.5f, 10f)]
        public float FireRate;

        [Range(1, 10)]
        public int Range;

        public TargetLine TargetLine;

        public UnityEvent<Enemy> OnEstablishedTarget;

        private TimeCounter _lastShotCounter;

        private Enemy _target;

        private AudioSource _audio;

        public bool ShowTargetLine { get; set; }

        public TargetMethod TargetMethod { get; set; }

        public bool CanAct { get; set; }

        private void Start()
        {
            if (TargetLine != null)
            {
                TargetLine.enabled = false;
            }

            _lastShotCounter = new(1 / FireRate);
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
                TargetLine.Ready();
            }
        }

        public void Survey(IEnumerable<GameObject> enemies)
        {
            _target = EstablishTarget(enemies);

            OnEstablishedTarget.Invoke(_target);

            if (TargetLine != null)
            {
                var isTargeting = _target != null && SourceTower.IsFiring();

                if (isTargeting && ShowTargetLine)
                {
                    TargetLine.Show(_target.transform.position);
                }
                else
                {
                    TargetLine.Hide();
                }
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
            var inRangeEnemies = enemies
                .Where(e => transform.GetDistanceToObject(e) <= Range)
                .Where(e => e.GetComponent<Enemy>().CanBeTargeted());

            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, TargetMethod);

            if (orderedEnemies.Any())
            {
                return orderedEnemies.First().GetComponent<Enemy>();
            }

            return null;
        }

        private void FireAtTarget()
        {
            var enoughTimePassed = !_lastShotCounter.IsRunning || _lastShotCounter.IsFinished;
            if (enoughTimePassed && _target != null)
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

            if (ExplosionPrefab != null)
            {
                projectile.OnStrike.AddListener(enemy =>
                {
                    var explosion = Instantiate(ExplosionPrefab, enemy.transform.position, Quaternion.identity)
                        .GetComponent<Explosion>();

                    var radius = StrikeProvider.GetRadius();
                    if (radius.HasValue)
                    {
                        explosion.SetRadius(radius.Value);
                    }
                });
            }

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

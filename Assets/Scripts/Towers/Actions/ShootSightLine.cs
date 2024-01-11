using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ShootSightLine : BaseBehaviour, ITowerAction, IHasFireRate, IHasRange
    {
        public TowerBehaviour SourceTower;

        public StrikeProvider StrikeProvider;

        [Range(0.1f, 10f)]
        public float FireRate;

        [Range(1, 10)]
        public int Range;

        // TODO: move this and related to its own script
        public LineRenderer TargetLine;

        public ShootLine ShootLine;

        public AudioSource AudioSource;

        public UnityEvent<Enemy> OnEstablishedTarget;

        public UnityEvent<Vector2> OnSetLine;

        private TimeCounter _lastShotCounter;

        private Enemy _target;

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

            logger = new(nameof(ShootSightLine));
        }

        public float GetFireRate() => FireRate;

        public int GetRange() => Range;

        public void Ready()
        {
            if (TargetLine != null)
            {
                TargetLine.SetPosition(0, transform.position);
            }

            ShootLine.Ready();
        }

        public void Survey(IEnumerable<GameObject> enemies)
        {
            _target = EstablishTarget(enemies);

            OnEstablishedTarget.Invoke(_target);

            var isTargeting = _target != null && SourceTower.IsFiring();

            if (isTargeting && TargetLine != null)
            {
                if (ShowTargetLine)
                {
                    TargetLine.enabled = true;
                    TargetLine.SetPosition(1, _target.transform.position);
                }
                else
                {
                    TargetLine.enabled = false;
                }
            }
        }

        public void Act()
        {
            if (CanAct)
            {
                _lastShotCounter.Increment(Time.deltaTime);

                var enoughTimePassed = !_lastShotCounter.IsRunning || _lastShotCounter.IsFinished;
                if (enoughTimePassed && _target != null)
                {
                    _lastShotCounter.Restart();
                    Shoot();
                }
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

        private void Shoot()
        {
            logger.Log("Shooting along sight line");

            var sightLine = GetSightLine(_target.transform);

            OnSetLine.Invoke(sightLine);

            if (AudioSource != null)
            {
                AudioSource.Play();
            }
        }

        private Vector2 GetSightLine(Transform t)
        {
            return -(transform.position - t.position).normalized * Range;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class AffectEnemy : BaseBehaviour, ITowerAction, IHasFireRate, IHasRange
    {
        public TowerBehaviour SourceTower;

        public EffectProvider EffectProvider;

        [Range(0.5f, 10f)]
        public float FireRate;

        [Range(1, 10)]
        public int Range;

        public LineRenderer TargetLine;

        public bool ShowTargetLine;

        private TimeCounter _lastEffectCounter;

        private Enemy _target;

        public TargetMethod TargetMethod { get; set; }

        public bool CanAct { get; set; }

        private void Start()
        {
            if (TargetLine != null)
            {
                TargetLine.enabled = false;
            }

            _lastEffectCounter = new(1f / FireRate);
            _lastEffectCounter.Start();

            logger = new MethodLogger(nameof(AffectEnemy));
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
                _lastEffectCounter.Increment(Time.deltaTime);

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
            // if enough time has passed since the last effect, trigger an effect
            if (_target != null && !_lastEffectCounter.IsRunning || _lastEffectCounter.IsFinished)
            {
                _lastEffectCounter.Restart();

                if (!_target.HasEffectCategory(EffectProvider.Category))
                {
                    logger.Log(EffectProvider.ApplyingEffect);
                    _target.AddEffect(EffectProvider.CreateEffect());
                }
                else
                {
                    // if the enemy is already affected, tough luck!
                    logger.Log(EffectProvider.EffectAlreadyApplied);
                }
            }
        }
    }
}

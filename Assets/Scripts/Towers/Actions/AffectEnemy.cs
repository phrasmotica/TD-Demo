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
        public EffectProvider EffectProvider;

        [Range(0.5f, 10f)]
        public float FireRate;

        [Range(1, 10)]
        public int Range;

        public LineRenderer TargetLine;

        public bool ShowTargetLine;

        private TimeCounter _lastEffectCounter;

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
            // need to execute this AFTER the parent tower has been placed
            // to get the correct position for the start of the line
            if (TargetLine != null)
            {
                TargetLine.SetPosition(0, transform.position);
            }
        }

        public void Act(IEnumerable<GameObject> enemies)
        {
            _lastEffectCounter.Increment(Time.deltaTime);

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
                var target = orderedEnemies.First().GetComponent<Enemy>();

                if (ShowTargetLine && TargetLine != null)
                {
                    TargetLine.enabled = true;
                    TargetLine.SetPosition(1, target.transform.position);
                }

                // if enough time has passed since the last effect, trigger an effect
                if (!_lastEffectCounter.IsRunning || _lastEffectCounter.IsFinished)
                {
                    _lastEffectCounter.Restart();

                    if (!target.HasEffectCategory(EffectProvider.Category))
                    {
                        logger.Log(EffectProvider.ApplyingEffect);
                        target.AddEffect(EffectProvider.CreateEffect());
                    }
                    else
                    {
                        // if the enemy is already affected, tough luck!
                        logger.Log(EffectProvider.EffectAlreadyApplied);
                    }
                }
            }
            else
            {
                TargetLine.enabled = false;
            }
        }
    }
}

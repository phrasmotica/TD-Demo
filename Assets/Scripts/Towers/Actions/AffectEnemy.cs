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

        private TimeCounter _lastEffectCounter;

        public TargetMethod TargetMethod { get; set; }

        public bool CanAct { get; set; }

        private void Start()
        {
            _lastEffectCounter = new(1f / FireRate);
            _lastEffectCounter.Start();

            logger = new MethodLogger(nameof(AffectEnemy));
        }

        public float GetFireRate() => FireRate;

        public int GetRange() => Range;

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

            // if there is an enemy in range and enough time has passed since the last effect, trigger an effect
            if (orderedEnemies.Any() && (!_lastEffectCounter.IsRunning || _lastEffectCounter.IsFinished))
            {
                _lastEffectCounter.Restart();

                var target = orderedEnemies.First().GetComponent<Enemy>();

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
    }
}

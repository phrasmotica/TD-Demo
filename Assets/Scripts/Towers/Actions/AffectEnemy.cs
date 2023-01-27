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

        public EffectSpecs Specs;

        private TimeCounter _lastEffectCounter;

        public float FireRate => Specs.FireRate;

        public int Range => Specs.Range;

        public bool CanAct { get; set; }

        private void Start()
        {
            _lastEffectCounter = new(1f / FireRate);
            _lastEffectCounter.Start();

            logger = new MethodLogger(nameof(AffectEnemy));
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
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, Specs.TargetMethod);

            // if there is an enemy in range and enough time has passed since the last effect, trigger an effect
            if (orderedEnemies.Any() && (!_lastEffectCounter.IsRunning || _lastEffectCounter.IsFinished))
            {
                _lastEffectCounter.Reset();

                var enemy = orderedEnemies.First().GetComponent<Enemy>();

                if (!enemy.HasEffectCategory(EffectProvider.Category))
                {
                    logger.Log(EffectProvider.ApplyingEffect);
                    enemy.AddEffect(EffectProvider.CreateEffect());
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

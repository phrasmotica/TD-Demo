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

        /// <summary>
        /// The time in seconds since the last effect was applied, or null if an effect has not been
        /// applied yet. Tracking this ensures the action can start firing immediately whenever an
        /// enemy appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? _timeSinceLastShot;

        public float FireRate => Specs.FireRate;

        public int Range => Specs.Range;

        public bool CanAct { get; set; }

        private void Start()
        {
            logger = new MethodLogger(nameof(AffectEnemy));
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

        private void CheckForEnemiesInRange(IEnumerable<GameObject> enemies)
        {
            var inRangeEnemies = enemies.Where(e => transform.GetDistanceToObject(e) <= Range);
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, Specs.TargetMethod);

            // if there is an enemy in range and enough time has passed since the last shot, fire a shot
            if (orderedEnemies.Any() && (!_timeSinceLastShot.HasValue || _timeSinceLastShot >= 1f / FireRate))
            {
                _timeSinceLastShot = 0;

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

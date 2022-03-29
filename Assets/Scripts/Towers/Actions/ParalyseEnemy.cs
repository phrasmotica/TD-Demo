using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Effects;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ParalyseEnemy : BaseBehaviour, ITowerAction, IHasFireRate, IHasRange
    {
        public EffectSpecs Specs;

        public TargetMethod TargetMethod;

        [Range(0.5f, 3f)]
        public float Duration;

        /// <summary>
        /// The time in seconds since the last shot was fired, or null if the tower has not fired a
        /// shot yet. Tracking this ensures a tower can start firing immediately whenever an enemy
        /// appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? _timeSinceLastShot;

        public float FireRate => Specs.FireRate;

        public int Range => Specs.Range;

        public bool CanAct { get; set; }

        private void Start()
        {
            logger = new MethodLogger(nameof(ParalyseEnemy));
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
            var orderedEnemies = EnemySorter.Sort(transform, inRangeEnemies, TargetMethod);

            if (orderedEnemies.Any())
            {
                var nearestEnemy = orderedEnemies.First();

                // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                if (!_timeSinceLastShot.HasValue || _timeSinceLastShot >= 1f / FireRate)
                {
                    _timeSinceLastShot = 0;

                    // if the enemy is already paralysed, tough luck!
                    var enemy = nearestEnemy.GetComponent<Enemy>();
                    if (!enemy.HasEffectCategory(EffectCategory.Paralyse))
                    {
                        logger.Log($"Paralysing enemy for {Duration} seconds.");
                        enemy.AddEffect(new Paralyse(Duration));
                    }
                    else
                    {
                        logger.Log("Enemy is already paralysed!");
                    }
                }
            }
        }
    }
}

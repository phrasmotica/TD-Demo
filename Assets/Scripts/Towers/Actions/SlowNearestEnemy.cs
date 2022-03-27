using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class SlowNearestEnemy : BaseBehaviour, ITowerAction, IHasFireRate, IHasRange, IHasShooting
    {
        public EffectSpecs Specs;

        [Range(0.5f, 1f)]
        public float Factor;

        [Range(0.5f, 3f)]
        public float Duration;

        /// <summary>
        /// The time in seconds since the last shot was fired, or null if the tower has not fired a
        /// shot yet. Tracking this ensures a tower can start firing immediately whenever an enemy
        /// appears instead of having to wait for a fixed cycle of its fire rate.
        /// </summary>
        private float? _timeSinceLastShot;

        public int FireRate => Specs.FireRate;

        public int Range => Specs.Range;

        public bool CanShoot { get; set; }

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

            if (CanShoot)
            {
                CheckForEnemiesInRange(enemies);
            }
        }

        private void CheckForEnemiesInRange(IEnumerable<GameObject> enemies)
        {
            var orderedEnemies = enemies.Where(e => transform.GetDistanceToObject(e) <= Range)
                                        .OrderBy(e => transform.GetDistanceToObject(e))
                                        .ToArray();

            if (orderedEnemies.Any())
            {
                var nearestEnemy = orderedEnemies.First();

                // if there is an enemy in range and enough time has passed since the last shot, fire a shot
                if (!_timeSinceLastShot.HasValue || _timeSinceLastShot >= 1f / FireRate)
                {
                    _timeSinceLastShot = 0;
                    StartCoroutine(Slow(nearestEnemy.GetComponent<Enemy>()));
                }
            }
        }

        private IEnumerator Slow(Enemy enemy)
        {
            if (!enemy.HasEffect(Effect.Slow))
            {
                enemy.AddEffect(Effect.Slow);

                var movement = enemy.GetComponent<WaypointFollower>();
                var oldSpeed = movement.Speed;

                movement.Speed = oldSpeed * Factor;

                yield return new WaitForSeconds(Duration);

                movement.Speed = oldSpeed;

                enemy.RemoveEffect(Effect.Slow);
            }
        }
    }
}

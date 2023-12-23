using System.Collections.Generic;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Towers.Effects;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Distractions
{
    public class DistractEnemy : BaseBehaviour
    {
        public Transform Source;

        [Range(0.5f, 3f)]
        public float Duration;

        [Range(0.05f, 0.5f)]
        public float Chance;

        private readonly List<int> _distractedEnemies = new();

        private void Start()
        {
            logger = new MethodLogger(nameof(DistractEnemy));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // TODO: only distract once the object has been placed down
            var otherObj = collision.gameObject;
            if (otherObj.CompareTag(Tags.Enemy))
            {
                var target = otherObj.GetComponent<Enemy>();
                var enemyId = target.gameObject.GetInstanceID();

                if (!_distractedEnemies.Contains(enemyId) && !target.HasEffectCategory(EffectCategory.Distract))
                {
                    var random = new System.Random();
                    if (random.NextDouble() < Chance)
                    {
                        logger.Log($"Distracting enemy {enemyId} for {Duration} seconds.");

                        target.AddEffect(new Distract(Duration)
                        {
                            Source = Source,
                        });

                        _distractedEnemies.Add(enemyId);
                    }
                    else
                    {
                        logger.Log($"Failed random check for distraction");
                    }
                }
                else
                {
                    // if the enemy is already distracted, tough luck!
                    logger.Log($"Enemy {enemyId} is already distracted!");
                }
            }
        }
    }
}

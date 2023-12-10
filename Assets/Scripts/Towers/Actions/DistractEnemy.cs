using System.Collections.Generic;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Towers.Effects;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class DistractEnemy : EffectProvider
    {
        public Transform Source;

        [Range(0.5f, 3f)]
        public float Duration;

        [Range(0.05f, 0.5f)]
        public float Chance;

        private readonly List<int> _distractedEnemies = new();

        public override EffectCategory Category => EffectCategory.Distract;

        public override string ApplyingEffect => $"Distracting enemy for {Duration} seconds.";

        public override string EffectAlreadyApplied => "Enemy is already distracted!";

        private void Start()
        {
            logger = new MethodLogger(nameof(DistractEnemy));
        }

        public override IEffect CreateEffect() => new Distract(Duration)
        {
            Source = Source,
        };

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;
            if (otherObj.CompareTag(Tags.Enemy))
            {
                var target = otherObj.GetComponent<Enemy>();
                var enemyId = target.gameObject.GetInstanceID();

                if (!_distractedEnemies.Contains(enemyId) && !target.HasEffectCategory(Category))
                {
                    var random = new System.Random();
                    if (random.NextDouble() < Chance)
                    {
                        logger.Log(ApplyingEffect);
                        target.AddEffect(CreateEffect());
                        _distractedEnemies.Add(enemyId);
                    }
                    else
                    {
                        logger.Log($"Failed random check for {Category} distraction");
                    }
                }
                else
                {
                    // if the enemy is already distracted, tough luck!
                    logger.Log(EffectAlreadyApplied);
                }
            }
        }
    }
}

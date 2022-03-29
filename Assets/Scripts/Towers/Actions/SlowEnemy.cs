using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class SlowEnemy : EffectProvider
    {
        [Range(0.5f, 1f)]
        public float Factor;

        [Range(0.5f, 3f)]
        public float Duration;

        public override EffectCategory Category => EffectCategory.Slow;

        public override string ApplyingEffect => $"Slowing enemy by {Factor} for {Duration} seconds.";

        public override string EffectAlreadyApplied => "Enemy is already slowed!";

        public override IEffect CreateEffect() => new SlowMovementSpeed(Factor, Duration);
    }
}

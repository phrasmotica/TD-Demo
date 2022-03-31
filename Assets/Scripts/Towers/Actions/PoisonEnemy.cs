using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class PoisonEnemy : EffectProvider
    {
        [Range(0.1f, 1f)]
        public float AmountPerSecond;

        [Range(0.5f, 2f)]
        public float Duration;

        public override EffectCategory Category => EffectCategory.Poison;

        public override string ApplyingEffect => $"Poisoning enemy at {AmountPerSecond} damage per second, for {Duration} seconds.";

        public override string EffectAlreadyApplied => "Enemy is already poisoned!";

        public override IEffect CreateEffect() => new Poison(AmountPerSecond, Duration);
    }
}

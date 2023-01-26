using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ParalyseEnemy : EffectProvider
    {
        [Range(0.5f, 3f)]
        public float Duration;

        public override EffectCategory Category => EffectCategory.Paralyse;

        public override string ApplyingEffect => $"Paralysing enemy for {Duration} seconds.";

        public override string EffectAlreadyApplied => "Enemy is already paralysed!";

        public override IEffect CreateEffect() => new Paralyse(Duration)
        {
            SourceTower = SourceTower,
        };
    }
}

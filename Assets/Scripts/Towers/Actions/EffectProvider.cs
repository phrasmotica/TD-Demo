using TDDemo.Assets.Scripts.Towers.Effects;
using TDDemo.Assets.Scripts.Util;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public abstract class EffectProvider : BaseBehaviour
    {
        public TowerBehaviour SourceTower;

        public abstract string ApplyingEffect { get; }

        public abstract string EffectAlreadyApplied { get; }

        public abstract EffectCategory Category { get; }

        public abstract IEffect CreateEffect();
    }
}

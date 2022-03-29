using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public abstract class EffectProvider : MonoBehaviour
    {
        public abstract string ApplyingEffect { get; }

        public abstract string EffectAlreadyApplied { get; }

        public abstract EffectCategory Category { get; }

        public abstract IEffect CreateEffect();
    }
}

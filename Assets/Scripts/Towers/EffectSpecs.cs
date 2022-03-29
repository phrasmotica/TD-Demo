using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class EffectSpecs : MonoBehaviour
    {
        [Range(0.5f, 10f)]
        public float FireRate;

        [Range(1, 10)]
        public int Range;

        public TargetMethod TargetMethod;
    }
}

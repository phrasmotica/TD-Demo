using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class EffectSpecs : MonoBehaviour
    {
        [Range(1, 10)]
        public int FireRate;

        [Range(1, 10)]
        public int Range;
    }
}

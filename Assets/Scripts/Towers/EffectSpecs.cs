using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    // TODO: absorb this into AffectEnemy script
    public class EffectSpecs : MonoBehaviour
    {
        [Range(0.5f, 10f)]
        public float FireRate;

        [Range(1, 10)]
        public int Range;
    }
}

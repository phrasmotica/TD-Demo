using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerLevel : MonoBehaviour
    {
        [Range(1, 10)]
        public int Cost;

        [Range(0.5f, 10f)]
        public float Time;
    }
}

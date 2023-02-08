using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class DropSpecs : MonoBehaviour
    {
        public GameObject DropPrefab;

        [Range(0f, 1f)]
        public float DropChance;
    }
}

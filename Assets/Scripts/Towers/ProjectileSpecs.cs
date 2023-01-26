using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class ProjectileSpecs : MonoBehaviour
    {
        public GameObject ProjectilePrefab;

        /// <summary>
        /// The initial velocity of shot projectiles.
        /// </summary>
        [Range(1, 10)]
        public int ProjectileSpeed;

        /// <summary>
        /// The rate of fire in shots per second.
        /// </summary>
        [Range(1, 10)]
        public int FireRate;

        [Range(1, 10)]
        public int Range;

        public TargetMethod TargetMethod;
    }
}

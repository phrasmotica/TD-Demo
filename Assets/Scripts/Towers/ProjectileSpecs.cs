using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class ProjectileSpecs : MonoBehaviour
    {
        /// <summary>
        /// The projectile prefab.
        /// </summary>
        public GameObject ProjectilePrefab;

        /// <summary>
        /// The damage of shot projectiles.
        /// </summary>
        [Range(1, 10)]
        public int Damage;

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

        /// <summary>
        /// The range of the projectile.
        /// </summary>
        [Range(1, 10)]
        public int Range;

        public TargetMethod TargetMethod;
    }
}

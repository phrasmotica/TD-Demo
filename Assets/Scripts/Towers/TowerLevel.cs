using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerLevel : MonoBehaviour
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
        [Range(1, 50)]
        public int Range;

        [Range(1, 10)]
        public int UpgradeCost;

        [Range(0.5f, 10f)]
        public float UpgradeTime;
    }
}

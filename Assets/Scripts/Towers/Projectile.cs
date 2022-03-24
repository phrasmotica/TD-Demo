using UnityEngine;
using TDDemo.Assets.Scripts.Extensions;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// The damage the projectile deals to an enemy.
        /// </summary>
        [Range(0, 3)]
        public int Damage;

        /// <summary>
        /// The position where the projectile was fired from.
        /// </summary>
        public Vector3 StartPosition { get; set; }

        /// <summary>
        /// The range of this projectile.
        /// </summary>
        public int Range { get; set; }

        private void Update()
        {
            // destroy this projectile once it's travelled its range
            if (transform.GetDistanceToPosition(StartPosition) > Range)
            {
                Destroy(gameObject);
            }
        }
    }
}

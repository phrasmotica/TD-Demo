using UnityEngine;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Towers
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
        public Vector3 StartPosition;

        /// <summary>
        /// The range of this projectile.
        /// </summary>
        public int Range;

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

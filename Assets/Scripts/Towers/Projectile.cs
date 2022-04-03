using UnityEngine;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Towers.Strikes;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Projectile : MonoBehaviour
    {
        public StrikeProvider StrikeProvider { get; set; }

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

        public IStrike CreateStrike() => StrikeProvider.CreateStrike();
    }
}

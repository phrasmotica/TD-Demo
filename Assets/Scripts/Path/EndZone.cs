using Assets.Scripts.Controller;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.Path
{
    public class EndZone : MonoBehaviour
    {
        /// <summary>
        /// The lives counter.
        /// </summary>
        public LivesCounter LivesCounter;

        /// <summary>
        /// When an enemy collides, deduct a life and destroy the enemy.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;

            var enemyComponent = otherObj.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                LivesCounter.AddLives(-1);
                Destroy(otherObj);
            }
        }
    }
}

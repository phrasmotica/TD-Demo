using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Enemies;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Path
{
    public class EndZone : MonoBehaviour
    {
        /// <summary>
        /// The lives controller.
        /// </summary>
        public LivesController LivesController;

        /// <summary>
        /// When an enemy collides, deduct a life and destroy the enemy.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;

            var enemyComponent = otherObj.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                LivesController.AddLives(-1);
                Destroy(otherObj);
            }
        }
    }
}

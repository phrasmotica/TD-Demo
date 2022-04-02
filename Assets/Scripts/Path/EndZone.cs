using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Path
{
    public class EndZone : MonoBehaviour
    {
        public LivesController LivesController;

        public WavesController WavesController;

        /// <summary>
        /// When an enemy collides, deduct a life and destroy the enemy.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;

            if (otherObj.CompareTag(Tags.Enemy))
            {
                LivesController.AddLives(-1);
                WavesController.RemoveEnemy(otherObj.GetComponent<Enemy>());
            }
        }
    }
}

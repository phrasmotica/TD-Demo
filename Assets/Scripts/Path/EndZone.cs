using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Path
{
    public class EndZone : MonoBehaviour
    {
        public event UnityAction<GameObject> OnEnemyCollide;

        /// <summary>
        /// When an enemy collides, deduct a life and destroy the enemy.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;

            if (otherObj.CompareTag(Tags.Enemy))
            {
                OnEnemyCollide?.Invoke(otherObj);
            }
        }
    }
}

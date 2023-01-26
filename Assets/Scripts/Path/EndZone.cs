using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Path
{
    public class EndZone : MonoBehaviour
    {
        public event UnityAction<Enemy> OnEnemyCollide;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;
            if (otherObj.CompareTag(Tags.Enemy))
            {
                OnEnemyCollide?.Invoke(otherObj.GetComponent<Enemy>());
            }
        }
    }
}

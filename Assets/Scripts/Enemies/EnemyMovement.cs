using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        /// <summary>
        /// The enemy's initial velocity.
        /// </summary>
        [Range(0, 10)]
        public int InitialVelocity;

        /// <summary>
        /// Give the enemy some velocity.
        /// </summary>
        void Start()
        {
            var rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(InitialVelocity, 0);
        }
    }
}

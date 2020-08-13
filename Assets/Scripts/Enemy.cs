using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// The enemy's starting health.
        /// </summary>
        [Range(1, 5)]
        public int StartingHealth;

        /// <summary>
        /// The money reward for killing this enemy.
        /// </summary>
        [Range(1, 10)]
        public int Reward;

        /// <summary>
        /// The enemy's current health.
        /// </summary>
        private int health;

        /// <summary>
        /// Delegate to run on death.
        /// </summary>
        public Action<int> OnKill { private get; set; }

        /// <summary>
        /// Set the enemy's health.
        /// </summary>
        private void Start()
        {
            health = StartingHealth;
        }

        /// <summary>
        /// Destroy this object if it's dead.
        /// </summary>
        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
                OnKill(Reward);
            }
        }

        /// <summary>
        /// If hit by a projectile, take damage and destroy the projectile.
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var otherObj = collision.gameObject;

            var projectileComponent = otherObj.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                health -= projectileComponent.Damage;
                Destroy(otherObj);
            }
        }
    }
}

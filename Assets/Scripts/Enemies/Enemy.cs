using System;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
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
        private int _health;

        /// <summary>
        /// Returns the enemy's current health as a decimal value between 0 and 1.
        /// </summary>
        public float HealthFraction => (float) _health / StartingHealth;

        /// <summary>
        /// Delegate to run on death.
        /// </summary>
        public Action<int> OnKill { private get; set; }

        /// <summary>
        /// Set the enemy's health.
        /// </summary>
        private void Start()
        {
            _health = StartingHealth;
        }

        /// <summary>
        /// Destroy this object if it's dead.
        /// </summary>
        private void Update()
        {
            if (_health <= 0)
            {
                Destroy(gameObject);
                OnKill(Reward);
            }
        }

        /// <summary>
        /// If hit by a projectile, take damage and destroy the projectile.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;

            var projectileComponent = otherObj.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                _health -= projectileComponent.Damage;
                PeekHealth();
                Destroy(otherObj);
            }
        }

        /// <summary>
        /// Show's the enemy's health briefly.
        /// </summary>
        private void PeekHealth()
        {
            GetComponentInChildren<HealthBar>().PeekHealth(HealthFraction);
        }
    }
}

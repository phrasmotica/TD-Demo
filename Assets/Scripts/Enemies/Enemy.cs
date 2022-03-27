using System;
using System.Collections.Generic;
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

        public AudioClip HurtAudio;
        public AudioClip DeadAudio;

        /// <summary>
        /// The enemy's current health.
        /// </summary>
        private int _health;

        private HashSet<Effect> _effects;

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
            _effects = new HashSet<Effect>();
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

                if (_health > 0)
                {
                    AudioSource.PlayClipAtPoint(HurtAudio, Vector3.zero);
                }
                else
                {
                    Kill();
                }
            }
        }

        public void AddEffect(Effect effect) => _effects.Add(effect);

        public bool HasEffect(Effect effect) => _effects.Contains(effect);

        public void RemoveEffect(Effect effect) => _effects.Remove(effect);

        /// <summary>
        /// Show's the enemy's health briefly.
        /// </summary>
        private void PeekHealth()
        {
            GetComponentInChildren<HealthBar>().PeekHealth(HealthFraction);
        }

        /// <summary>
        /// Destroy this object if it's dead.
        /// </summary>
        private void Kill()
        {
            AudioSource.PlayClipAtPoint(DeadAudio, Vector3.zero);

            Destroy(gameObject);
            OnKill(Reward);
        }
    }
}

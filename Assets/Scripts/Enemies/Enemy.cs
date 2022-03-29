using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// The effects present on this enemy.
        /// </summary>
        private List<IEffect> _effects;

        /// <summary>
        /// Returns the enemy's current health as a decimal value between 0 and 1.
        /// </summary>
        public float HealthFraction => (float) _health / StartingHealth;

        /// <summary>
        /// Delegate to run on receiving damage.
        /// </summary>
        public event Action<float> OnHurt;

        /// <summary>
        /// Delegate to run on an effect being inflicted.
        /// </summary>
        public event Action<IEffect> OnEffect;

        /// <summary>
        /// Delegate to run on death.
        /// </summary>
        public event Action<int> OnKill;

        /// <summary>
        /// Set the enemy's health.
        /// </summary>
        private void Start()
        {
            _health = StartingHealth;
            _effects = new List<IEffect>();
        }

        private void Update()
        {
            foreach (var e in _effects)
            {
                e.Update(this, Time.deltaTime);

                if (e.IsFinished)
                {
                    e.End(this);
                }
            }

            // clean up finished effects
            _effects = _effects.Where(e => !e.IsFinished).ToList();
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
                OnHurt(HealthFraction);
                Destroy(otherObj);

                if (_health > 0)
                {
                    AudioSource.PlayClipAtPoint(HurtAudio, Vector3.zero);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(DeadAudio, Vector3.zero);

                    OnKill(Reward);
                    Destroy(gameObject);
                }
            }
        }

        public void AddEffect(IEffect effect)
        {
            _effects.Add(effect);
            effect.Start(this);
            OnEffect(effect);
        }

        public bool HasEffectCategory(EffectCategory category) => _effects.Any(e => e.Category == category);
    }
}

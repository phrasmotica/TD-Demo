using System;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Range(1, 5)]
        public float StartingHealth;

        [Range(1, 10)]
        public int BaseGoldReward;

        [Range(1, 5)]
        public int BaseXpReward;

        [Range(1, 10)]
        public int Strength;

        public AudioClip HurtAudio;
        public AudioClip HealAudio;
        public AudioClip DeadAudio;

        private float _health;

        private List<IEffect> _effects;

        public TowerBehaviour LastDamagingTower { get; set; }

        public float HealthFraction => _health / StartingHealth;

        public UnityEvent OnMouseEnterEvent;

        public UnityEvent OnMouseExitEvent;

        public UnityEvent<Enemy> OnHurt;

        public UnityEvent<Enemy> OnHeal;

        public UnityEvent<Enemy, IEffect> OnEffect;

        public UnityEvent<Enemy> OnPreKill;

        public UnityEvent<Enemy, TowerBehaviour> OnKill;

        private void Start()
        {
            _health = StartingHealth;
            _effects = new List<IEffect>();
        }

        private void Update()
        {
            if (_health <= 0)
            {
                AudioSource.PlayClipAtPoint(DeadAudio, Vector3.zero);
                LastDamagingTower.GainKill();
                LastDamagingTower.GainXp(BaseXpReward);

                // required for things that need to happen before the game object is destroyed
                OnPreKill.Invoke(this);

                OnKill.Invoke(this, LastDamagingTower);
                return;
            }

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

        private void OnMouseEnter() => OnMouseEnterEvent?.Invoke();

        private void OnMouseExit() => OnMouseExitEvent?.Invoke();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;
            if (otherObj.TryGetComponent<Projectile>(out var projectileComponent))
            {
                var strike = projectileComponent.CreateStrike();
                strike.Apply(this);

                Destroy(otherObj);
            }
        }

        public void TakeDamage(float amount, bool isFromEffect)
        {
            _health -= amount;

            if (!isFromEffect && _health > 0)
            {
                AudioSource.PlayClipAtPoint(HurtAudio, Vector3.zero);
            }

            OnHurt.Invoke(this);
        }

        public void Heal(float amount)
        {
            _health += amount;

            if (_health > 0)
            {
                AudioSource.PlayClipAtPoint(HealAudio, Vector3.zero);
            }

            OnHeal.Invoke(this);
        }

        public void AddEffect(IEffect effect)
        {
            _effects.Add(effect);
            effect.Start(this);
            OnEffect.Invoke(this, effect);
        }

        public bool HasEffectCategory(EffectCategory category) => _effects.Any(e => e.Category == category);
    }
}

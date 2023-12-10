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
        [Range(1, 20)]
        public float StartingHealth;

        [Range(1, 10)]
        public int BaseGoldReward;

        [Range(1, 5)]
        public int BaseXpReward;

        [Range(1, 10)]
        public int Strength;

        public Transform SpriteTransform;

        public AudioSource AudioSource;

        public AudioClip DistractedAudio;
        public AudioClip HurtAudio;
        public AudioClip HealAudio;
        public AudioClip DeadAudio;

        private float _health;

        private List<IEffect> _effects;

        private bool _isDead;

        public TowerBehaviour LastDamagingTower { get; set; }

        public float HealthFraction => _health / StartingHealth;

        public UnityEvent OnMouseEnterEvent;

        public UnityEvent OnMouseExitEvent;

        public UnityEvent<Enemy> OnHurt;

        public UnityEvent<Enemy> OnHeal;

        public UnityEvent<Enemy, IEffect> OnEffect;

        public UnityEvent<Enemy, TowerBehaviour> OnKill;

        private void Start()
        {
            _health = StartingHealth;
            _effects = new List<IEffect>();
        }

        private void Update()
        {
            if (_health <= 0 && !_isDead)
            {
                StartCoroutine(DoDie());
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
            if (CanBeTargeted() && otherObj.TryGetComponent<Projectile>(out var projectileComponent))
            {
                var strike = projectileComponent.CreateStrike();
                strike.Apply(this);

                Destroy(otherObj);
            }
        }

        public bool CanBeTargeted() => !_isDead;

        public void TakeDamage(float amount, bool isFromEffect)
        {
            _health -= amount;

            if (!isFromEffect && _health > 0)
            {
                AudioSource.PlayOneShot(HurtAudio);
            }

            OnHurt.Invoke(this);
        }

        public void Heal(float amount)
        {
            _health += amount;

            if (_health > 0)
            {
                AudioSource.PlayOneShot(HealAudio);
            }

            OnHeal.Invoke(this);
        }

        public void AddEffect(IEffect effect)
        {
            _effects.Add(effect);
            effect.Start(this);

            if (effect.Category == EffectCategory.Distract)
            {
                PlaySound(DistractedAudio);
            }

            OnEffect.Invoke(this, effect);
        }

        public bool HasEffectCategory(EffectCategory category) => _effects.Any(e => e.Category == category);

        private System.Collections.IEnumerator DoDie()
        {
            _isDead = true;

            LastDamagingTower.GainKill();
            LastDamagingTower.GainXp(BaseXpReward);

            // required before the game object is destroyed
            OnKill.Invoke(this, LastDamagingTower);

            AudioSource.PlayOneShot(DeadAudio);

            while (AudioSource.isPlaying)
            {
                yield return null;
            }

            Destroy(gameObject);
        }

        private void PlaySound(AudioClip clip)
        {
            AudioSource.clip = clip;
            AudioSource.pitch = 1 + 0.2f * (float) (new System.Random().NextDouble() - 0.5); // some randomness
            AudioSource.Play();
        }
    }
}

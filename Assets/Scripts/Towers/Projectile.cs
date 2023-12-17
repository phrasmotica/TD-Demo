using UnityEngine;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Actions;
using System.Collections;
using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Projectile : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public AudioSource AudioSource;

        public StrikeProvider StrikeProvider { get; set; }

        public Vector3 StartPosition { get; set; }

        public int Range { get; set; }

        private void Update()
        {
            // destroy this projectile once it's travelled its range
            if (transform.GetDistanceToPosition(StartPosition) > Range)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;
            if (otherObj.TryGetComponent<Enemy>(out var enemy) && enemy.CanBeTargeted())
            {
                var strike = StrikeProvider.CreateStrike();
                strike.Apply(enemy);

                StartCoroutine(DoStrike());
            }
        }

        private IEnumerator DoStrike()
        {
            SpriteRenderer.enabled = false;

            if (AudioSource != null)
            {
                AudioSource.pitch = 1 + 0.2f * (float) (new System.Random().NextDouble() - 0.5); // some randomness
                AudioSource.Play();

                while (AudioSource.isPlaying)
                {
                    yield return null;
                }
            }

            Destroy(gameObject);
        }
    }
}

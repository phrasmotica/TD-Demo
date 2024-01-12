using System.Collections;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ShootLineCollider : BaseBehaviour
    {
        public BoxCollider2D Collider;

        public AudioSource AudioSource;

        public StrikeProvider StrikeProvider;

        [Range(0.1f, 5f)]
        public float LifetimeSeconds;

        public UnityEvent<Enemy> OnStrike;

        private void Start()
        {
            logger = new(nameof(ShootLineCollider));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var otherObj = collision.gameObject;

            logger.Log($"{Time.frameCount} Colliding with {otherObj.name}");

            // TODO: ensure enemies cannot walk into the line after it's been fired
            if (otherObj.TryGetComponent<Enemy>(out var enemy) && enemy.CanBeTargeted())
            {
                StartCoroutine(DoStrike(enemy));
            }
        }

        private IEnumerator DoStrike(Enemy enemy)
        {
            var strike = StrikeProvider.CreateStrike();
            strike.Apply(enemy);

            OnStrike.Invoke(enemy);

            if (AudioSource != null)
            {
                AudioSource.pitch = 1 + 0.2f * (float) (new System.Random().NextDouble() - 0.5); // some randomness
                AudioSource.Play();

                while (AudioSource.isPlaying)
                {
                    yield return null;
                }
            }
        }

        public void UpdateCollider(Vector2 line)
        {
            // ensure collider lies on the line
            Collider.enabled = true;
            Collider.offset = new(line.magnitude / 2, Collider.offset.y);
            Collider.size = new(line.magnitude, Collider.size.y);

            var direction = ((Vector3) line).normalized;
            Collider.transform.right = direction;

            StartCoroutine(HideCollider());
        }

        private IEnumerator HideCollider()
        {
            // wait so that collision detection can happen - seems to take about 4 frames on average
            // for this to occur
            yield return new WaitForSeconds(LifetimeSeconds);

            Collider.enabled = false;
        }
    }
}

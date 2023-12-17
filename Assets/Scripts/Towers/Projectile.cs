using UnityEngine;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Towers.Strikes;
using System.Collections;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Projectile : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public AudioSource AudioSource;

        public bool HasStruck {  get; private set; }

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

        public IStrike CreateStrike() => StrikeProvider.CreateStrike();

        public IEnumerator DoStrike()
        {
            HasStruck = true;

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

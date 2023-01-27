using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class EphemeralText : MonoBehaviour
    {
        [Range(0.5f, 1f)]
        public float LifeTimeSeconds;

        [Range(0.5f, 2f)]
        public float Velocity;

        private TimeCounter _counter;

        private void Start()
        {
            _counter = new(LifeTimeSeconds);
            _counter.Start();
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, Velocity * Time.deltaTime));
            _counter.Increment(Time.deltaTime);

            if (_counter.IsFinished)
            {
                Destroy(gameObject);
            }
        }
    }
}

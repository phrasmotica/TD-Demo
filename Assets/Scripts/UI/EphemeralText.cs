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

        private TimeCounter _timeCounter;

        private void Start()
        {
            _timeCounter = new TimeCounter(LifeTimeSeconds);
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, Velocity * Time.deltaTime));
            _timeCounter.Increment(Time.deltaTime);

            if (_timeCounter.IsFinished)
            {
                Destroy(gameObject);
            }
        }
    }
}

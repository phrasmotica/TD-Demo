using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Distractions
{
    public class Lifetime : BaseBehaviour
    {
        [Range(5f, 30f)]
        public float LifetimeSeconds;

        private TimeCounter _counter;

        private void Start()
        {
            _counter = new(LifetimeSeconds);
            _counter.Start();

            logger = new MethodLogger(nameof(Lifetime));
        }

        private void Update()
        {
            _counter.Increment(Time.deltaTime);

            if (_counter.IsFinished)
            {
                logger.Log($"{gameObject.name} has expired, destroying");

                Destroy(gameObject);
            }
        }
    }
}

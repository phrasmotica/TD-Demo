using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers
{
    public class SlowMovementSpeed : IEffect
    {
        private readonly float _factor;
        private readonly float _duration;

        private float _initialSpeed;
        private TimeCounter _counter;

        public EffectCategory Category => EffectCategory.Slow;

        public float Progress => _counter.Progress;

        public bool IsFinished => _counter.IsFinished;

        public SlowMovementSpeed(float factor, float duration)
        {
            _factor = factor;
            _duration = duration;
        }

        public void Start(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            _initialSpeed = movement.Speed;

            movement.Speed = _initialSpeed * _factor;

            _counter = new TimeCounter(_duration);
        }

        public void Update(Enemy enemy, float time)
        {
            _counter.Increment(time);
        }

        public void End(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            movement.Speed = _initialSpeed;
        }
    }
}

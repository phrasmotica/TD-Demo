using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers.Effects
{
    public class Paralyse : IEffect
    {
        private readonly float _duration;

        private TimeCounter _counter;

        public EffectCategory Category => EffectCategory.Paralyse;

        public float Progress => _counter.Progress;

        public bool IsFinished => _counter.IsFinished;

        public Paralyse(float duration)
        {
            _duration = duration;
        }

        public void Start(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            movement.IsParalysed = true;

            _counter = new(_duration);
        }

        public void Update(Enemy enemy, float time)
        {
            _counter.Increment(time);
        }

        public void End(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            movement.IsParalysed = false;
        }
    }
}

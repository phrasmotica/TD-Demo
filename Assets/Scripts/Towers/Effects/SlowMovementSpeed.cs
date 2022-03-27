using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers
{
    public class SlowMovementSpeed : IEffect
    {
        private readonly float _factor;

        private float _initialSpeed;

        public SlowMovementSpeed(float factor)
        {
            _factor = factor;
        }

        public EffectCategory Category => EffectCategory.Slow;

        public void Apply(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            _initialSpeed = movement.Speed;

            movement.Speed = _initialSpeed * _factor;
        }

        public void Remove(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            movement.Speed = _initialSpeed;
        }
    }
}

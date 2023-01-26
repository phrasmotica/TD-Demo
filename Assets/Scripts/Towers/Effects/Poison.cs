using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers.Effects
{
    public class Poison : IEffect
    {
        private readonly float _amountPerSecond;
        private readonly float _duration;

        private TimeCounter _counter;

        public TowerBehaviour SourceTower { get; set; }

        public EffectCategory Category => EffectCategory.Poison;

        public float Progress => _counter.Progress;

        public bool IsFinished => _counter.IsFinished;

        public Poison(float amount, float duration)
        {
            _amountPerSecond = amount;
            _duration = duration;
        }

        public void Start(Enemy enemy)
        {
            _counter = new(_duration);
        }

        public void Update(Enemy enemy, float time)
        {
            _counter.Increment(time);

            var amount = _amountPerSecond * time;
            enemy.TakeDamage(amount);
        }

        public void End(Enemy enemy)
        {
        }
    }
}

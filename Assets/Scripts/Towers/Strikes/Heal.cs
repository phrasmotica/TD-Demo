using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers.Strikes
{
    public class Heal : IStrike
    {
        private readonly float _amount;

        public Heal(float amount)
        {
            _amount = amount;
        }

        public void Apply(Enemy enemy)
        {
            enemy.Heal(_amount);
        }
    }
}

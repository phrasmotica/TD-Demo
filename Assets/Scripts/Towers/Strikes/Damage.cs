using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers.Strikes
{
    public class Damage : IStrike
    {
        private readonly float _amount;

        public TowerBehaviour SourceTower { get; set; }

        public Damage(float amount)
        {
            _amount = amount;
        }

        public void Apply(Enemy enemy)
        {
            enemy.TakeDamage(_amount, false);
            enemy.LastDamagingTower = SourceTower;
        }
    }
}

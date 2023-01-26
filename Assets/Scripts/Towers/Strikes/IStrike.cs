using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers.Strikes
{
    public interface IStrike
    {
        TowerBehaviour SourceTower { get; set; }

        void Apply(Enemy enemy);
    }
}

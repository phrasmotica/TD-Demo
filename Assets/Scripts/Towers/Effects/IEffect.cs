using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers.Effects
{
    public interface IEffect
    {
        TowerBehaviour SourceTower { get; set; }

        EffectCategory Category { get; }

        float Progress { get; }

        bool IsFinished { get; }

        void Start(Enemy enemy);

        void Update(Enemy enemy, float time);

        void End(Enemy enemy);
    }
}

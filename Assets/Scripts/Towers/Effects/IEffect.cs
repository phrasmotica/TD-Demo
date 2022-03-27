using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers
{
    public interface IEffect
    {
        EffectCategory Category { get; }

        void Apply(Enemy enemy);

        void Remove(Enemy enemy);
    }

    public enum EffectCategory
    {
        Slow,
    }
}

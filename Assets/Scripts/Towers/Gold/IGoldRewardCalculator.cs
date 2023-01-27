namespace TDDemo.Assets.Scripts.Towers.Gold
{
    public interface IGoldRewardCalculator<TSource>
    {
        int Compute(TSource source, int baseReward);
    }
}

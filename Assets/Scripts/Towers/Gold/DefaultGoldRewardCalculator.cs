namespace TDDemo.Assets.Scripts.Towers.Gold
{
    public class DefaultGoldRewardCalculator<TSource> : IGoldRewardCalculator<TSource>
    {
        public int Compute(TSource source, int baseReward) => baseReward;
    }
}

namespace TDDemo.Assets.Scripts.Towers.Gold
{
    public class LevelLinearGoldRewardCalculator : IGoldRewardCalculator<Tower>
    {
        public int Compute(Tower tower, int baseReward) => tower.Level * baseReward;
    }
}

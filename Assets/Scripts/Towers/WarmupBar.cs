namespace TDDemo.Assets.Scripts.Towers
{
    public class WarmupBar : TowerProgressBar
    {
        protected override bool ShouldDraw() => Tower.IsWarmingUp();
    }
}

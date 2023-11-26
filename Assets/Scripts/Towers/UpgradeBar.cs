namespace TDDemo.Assets.Scripts.Towers
{
    public class UpgradeBar : TowerProgressBar
    {
        protected override bool ShouldDraw() => Tower.IsUpgrading();
    }
}

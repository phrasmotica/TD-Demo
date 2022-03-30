using System;

namespace TDDemo.Assets.Scripts.Towers
{
    public class UpgradeBar : TowerProgressBar
    {
        protected override void SetEventHandler(Action<float> handler)
        {
            Tower.OnUpgradeProgress += handler;
        }

        protected override bool ShouldDraw() => Tower.IsUpgrading();
    }
}

using System;

namespace TDDemo.Assets.Scripts.Towers
{
    public class WarmupBar : TowerProgressBar
    {
        protected override void SetEventHandler(Action<float> handler)
        {
            Tower.OnWarmupProgress += handler;
        }

        protected override bool ShouldDraw() => Tower.IsWarmingUp();
    }
}

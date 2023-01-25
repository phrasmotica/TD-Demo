using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers
{
    public class WarmupBar : TowerProgressBar
    {
        protected override void SetEventHandler(UnityAction<float> handler)
        {
            Tower.OnWarmupProgress += handler;
        }

        protected override bool ShouldDraw() => Tower.IsWarmingUp();
    }
}

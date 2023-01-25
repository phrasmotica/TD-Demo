using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers
{
    public class UpgradeBar : TowerProgressBar
    {
        protected override void SetEventHandler(UnityAction<float> handler)
        {
            Tower.OnUpgradeProgress += handler;
        }

        protected override bool ShouldDraw() => Tower.IsUpgrading();
    }
}

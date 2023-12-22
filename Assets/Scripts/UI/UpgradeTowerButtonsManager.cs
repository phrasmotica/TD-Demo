using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTowerButtonsManager : MonoBehaviour
    {
        public List<UpgradeTower> Buttons;

        public List<UpgradeTooltipOnHover> Tooltips;

        public void SetTower(TowerBehaviour tower)
        {
            foreach (var button in Buttons)
            {
                button.SetTower(tower);
            }

            if (tower != null)
            {
                foreach (var tooltip in Tooltips)
                {
                    var (canUpgrade, upgrade) = tower.GetUpgradeInfo(tooltip.UpgradeIndex);
                    tooltip.Upgrade = upgrade;
                }
            }
        }

        public void Clear()
        {
            foreach (var button in Buttons)
            {
                button.Clear();
            }

            foreach (var tooltip in Tooltips)
            {
                tooltip.Upgrade = null;
            }
        }
    }
}

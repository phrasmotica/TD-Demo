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

            foreach (var tooltip in Tooltips)
            {
                tooltip.SetUpgradeFromTower(tower);
            }
        }

        public void Refresh()
        {
            foreach (var button in Buttons)
            {
                button.Refresh();
            }

            // TODO: update tooltips
        }

        public void Clear()
        {
            foreach (var button in Buttons)
            {
                button.Clear();
            }

            foreach (var tooltip in Tooltips)
            {
                tooltip.SetUpgradeFromTower(null);
            }
        }
    }
}

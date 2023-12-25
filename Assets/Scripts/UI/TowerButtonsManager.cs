using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerButtonsManager : MonoBehaviour
    {
        public List<UpgradeTower> UpgradeButtons;

        public List<UpgradeTooltipOnHover> UpgradeTooltips;

        public List<SellTower> SellButtons;

        public List<SellTooltipOnHover> SellTooltips;

        private void Start()
        {
            foreach (var button in UpgradeButtons)
            {
                button.gameObject.SetActive(false);
            }

            foreach (var button in SellButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        public void SetTower(TowerBehaviour tower)
        {
            foreach (var button in UpgradeButtons)
            {
                button.gameObject.SetActive(tower != null);
                button.SetTower(tower);
            }

            foreach (var tooltip in UpgradeTooltips)
            {
                tooltip.SetUpgradeFromTower(tower);
            }

            foreach (var button in SellButtons)
            {
                button.gameObject.SetActive(tower != null);
                button.SetState(tower);
            }

            foreach (var tooltip in SellTooltips)
            {
                tooltip.SetTower(tower);
            }
        }

        public void Refresh()
        {
            foreach (var button in UpgradeButtons)
            {
                button.Refresh();
            }
        }

        public void Clear()
        {
            foreach (var button in UpgradeButtons)
            {
                button.gameObject.SetActive(false);
                button.Clear();
            }

            foreach (var tooltip in UpgradeTooltips)
            {
                tooltip.SetUpgradeFromTower(null);
            }

            foreach (var button in SellButtons)
            {
                button.gameObject.SetActive(false);
                button.SetState(null);
            }

            foreach (var tooltip in SellTooltips)
            {
                tooltip.SetTower(null);
            }
        }
    }
}

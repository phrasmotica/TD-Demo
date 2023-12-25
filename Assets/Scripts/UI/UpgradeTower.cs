using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Upgrades;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTower : MonoBehaviour
    {
        public BankManager Bank;

        public int UpgradeIndex;

        public Button Button;

        public Image UpgradeImage;

        public Image ArrowImage;

        public TowerController TowerController;

        private TowerBehaviour _tower;

        public void Upgrade() => TowerController.UpgradeSelectedTower(UpgradeIndex);

        public void SetTower(TowerBehaviour tower)
        {
            _tower = tower;

            Refresh();
        }

        public void Clear() => SetTower(null);

        public void Refresh()
        {
            if (_tower != null)
            {
                var (canUpgrade, upgrade) = _tower.GetUpgradeInfo(UpgradeIndex);
                canUpgrade &= !_tower.IsUpgrading();

                var canAfford = upgrade != null && Bank.CanAfford(upgrade.Price) != PurchaseMethod.None;

                Button.interactable = canUpgrade && canAfford;

                var colour = ComputeImageColour(canUpgrade, canAfford, upgrade);

                UpgradeImage.color = colour;
                UpgradeImage.sprite = upgrade != null ? upgrade.Sprite : null;

                ArrowImage.color = colour;
                ArrowImage.gameObject.SetActive(true);
            }
            else
            {
                Button.interactable = false;

                UpgradeImage.color = ColourHelper.ZeroOpacity;
                UpgradeImage.sprite = null;

                ArrowImage.color = ColourHelper.ZeroOpacity;
                ArrowImage.gameObject.SetActive(false);
            }
        }

        private static Color ComputeImageColour(bool canUpgrade, bool canAfford, UpgradeNode upgrade)
        {
            if (canUpgrade && canAfford)
            {
                return ColourHelper.FullOpacity;
            }

            if (upgrade != null)
            {
                return ColourHelper.HalfOpacity;
            }

            return ColourHelper.ZeroOpacity;
        }
    }
}

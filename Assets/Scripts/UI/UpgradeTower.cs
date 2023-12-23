using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTower : MonoBehaviour
    {
        public BankManager Bank;

        public PurchaseMethod PurchaseMethod;

        public int UpgradeIndex;

        public Button Button;

        public Image Image;

        public TowerController TowerController;

        private TowerBehaviour _tower;

        public void Upgrade() => TowerController.UpgradeSelectedTower(PurchaseMethod, UpgradeIndex);

        public void SetTower(TowerBehaviour tower)
        {
            _tower = tower;

            Refresh();
        }

        public void Clear() => SetTower(null);

        public void Refresh()
        {
            if (_tower != null && !_tower.IsUpgrading())
            {
                var (canUpgrade, upgrade) = _tower.GetUpgradeInfo(UpgradeIndex);
                var canAfford = upgrade != null && Bank.CanAffordVia(upgrade.Price, PurchaseMethod);

                Button.interactable = canUpgrade && canAfford;

                Image.color = canUpgrade && canAfford ? 
                    ColourHelper.FullOpacity : 
                    upgrade != null ? 
                        ColourHelper.HalfOpacity : 
                        ColourHelper.ZeroOpacity;

                Image.sprite = canAfford ? upgrade.Sprite : null;
            }
            else
            {
                Button.interactable = false;
                Image.color = ColourHelper.ZeroOpacity;
                Image.sprite = null;
            }
        }
    }
}

using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    // TODO: create tooltip prefab for when you hover over an upgrade button
    // that shows the upgrade cost
    public class UpgradeTower : MonoBehaviour
    {
        public BankManager Bank;

        public PurchaseMethod PurchaseMethod;

        public int UpgradeIndex;

        public Button Button;

        public SpriteRenderer Sprite;

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
                var (canUpgrade, price) = _tower.GetUpgradeInfo(UpgradeIndex);
                var canAfford = Bank.CanAffordVia(price, PurchaseMethod);

                Button.interactable = canUpgrade && canAfford;
                Sprite.color = canUpgrade && canAfford ? ColourHelper.FullOpacity : ColourHelper.HalfOpacity;
            }
            else
            {
                Button.interactable = false;
                Sprite.color = ColourHelper.HalfOpacity;
            }
        }
    }
}

using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
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

        public SpriteRenderer Sprite;

        public TowerController TowerController;

        private TowerBehaviour _tower;

        public void Upgrade() => TowerController.UpgradeSelectedTower(PurchaseMethod);

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
                var (canUpgrade, price) = _tower.GetUpgradeInfo();
                var canAfford = Bank.CanAffordVia(price, PurchaseMethod);

                GetComponent<Button>().interactable = canUpgrade && canAfford;
                Sprite.color = canUpgrade && canAfford ? Color.white : Color.grey;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                Sprite.color = Color.grey;
            }
        }
    }
}

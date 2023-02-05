using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTower : MonoBehaviour
    {
        private const string CouponText = "Upgrade (    )";

        public BankManager Bank;

        public TowerController TowerController;

        public SpriteRenderer SpriteRenderer;

        private TowerBehaviour _tower;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(TowerController.UpgradeSelectedTower);

            Bank.OnMoneyChange += money => Refresh();
            Bank.OnCouponsChange += coupons => Refresh();
            Bank.OnChangeUseCoupons += useCoupons => Refresh();

            TowerController.OnStartUpgradeSelectedTower += SetTower;
            TowerController.OnFinishUpgradeSelectedTower += SetTower;
            TowerController.OnChangeSelectedTower += SetTower;
            TowerController.OnSellSelectedTower += SetTower;
        }

        private void SetTower(TowerBehaviour tower)
        {
            _tower = tower;

            Refresh();
        }

        private void Refresh()
        {
            if (_tower != null && !_tower.IsUpgrading())
            {
                var (canUpgrade, price) = _tower.GetUpgradeInfo();
                var purchaseMethod = Bank.CanAfford(price);

                GetComponent<Button>().interactable = canUpgrade && purchaseMethod != PurchaseMethod.None;
                GetComponentInChildren<Text>().text = canUpgrade ? ComputeText(purchaseMethod, price) : "Upgrade";

                SpriteRenderer.enabled = canUpgrade && purchaseMethod == PurchaseMethod.Coupons;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponentInChildren<Text>().text = "Upgrade";

                SpriteRenderer.enabled = false;
            }
        }

        private string ComputeText(PurchaseMethod purchaseMethod, int price) => purchaseMethod switch
        {
            PurchaseMethod.Coupons => CouponText,
            _ => $"Upgrade ({price})",
        };
    }
}

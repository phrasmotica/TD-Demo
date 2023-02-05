using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class CreateTower : MonoBehaviour
    {
        public GameObject TowerPrefab;

        public BankManager Bank;

        public TowerController TowerController;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => TowerController.CreateNewTower(TowerPrefab));

            Bank.OnMoneyChange += money => SetInteractable();
            Bank.OnCouponsChange += coupons => SetInteractable();
            Bank.OnChangeUseCoupons += useCoupons => SetInteractable();
        }

        private void SetInteractable()
        {
            var tower = TowerPrefab.GetComponent<TowerBehaviour>();
            var canAfford = Bank.CanAffordToBuy(tower) != PurchaseMethod.None;
            GetComponent<Button>().interactable = canAfford;
        }
    }
}

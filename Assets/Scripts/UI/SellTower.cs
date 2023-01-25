using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public MoneyController MoneyController;

        public TowerController TowerController;

        public event UnityAction OnSell;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Sell);

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;
            TowerController.OnChangeSelectedTower += SetState;
        }

        private void Sell() => OnSell?.Invoke();

        private void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;
            GetComponent<Button>().interactable = canSellTower;

            if (canSellTower)
            {
                var sellPrice = MoneyController.GetSellPrice(tower);
                GetComponentInChildren<Text>().text = $"Sell ({sellPrice})";
            }
            else
            {
                GetComponentInChildren<Text>().text = "Sell";
            }
        }
    }
}

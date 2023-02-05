using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public BankManager Bank;

        public TowerController TowerController;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(TowerController.SellSelectedTower);

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;
            TowerController.OnChangeSelectedTower += SetState;
        }

        private void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;
            GetComponent<Button>().interactable = canSellTower;

            if (canSellTower)
            {
                var sellPrice = Bank.GetSellPrice(tower);
                GetComponentInChildren<Text>().text = $"Sell ({sellPrice})";
            }
            else
            {
                GetComponentInChildren<Text>().text = "Sell";
            }
        }
    }
}

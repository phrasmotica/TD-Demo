using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public TowerController TowerController;

        public TowerManager TowerManager;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Sell);

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;

            TowerManager.OnSelectedTowerChange += SetState;
        }

        private void Sell() => TowerController.SellSelectedTower();

        private void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;
            GetComponent<Button>().interactable = canSellTower;

            var sellPrice = TowerController.GetSellPrice(tower);
            var text = canSellTower && sellPrice.HasValue ? $"Sell ({sellPrice.Value})" : "Sell";
            GetComponentInChildren<Text>().text = text;
        }
    }
}

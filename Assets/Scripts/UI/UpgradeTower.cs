using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTower : MonoBehaviour
    {
        public MoneyController MoneyController;

        public TowerController TowerController;

        private TowerBehaviour _tower;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(TowerController.UpgradeSelectedTower);

            MoneyController.OnMoneyChange += SetInteractable;

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;
            TowerController.OnChangeSelectedTower += SetState;
            TowerController.OnSellSelectedTower += SetState;
        }

        private void SetState(TowerBehaviour tower)
        {
            _tower = tower;

            if (_tower != null)
            {
                var canUpgrade = _tower.CanBeUpgraded();
                var canAfford = MoneyController.CanAffordToUpgrade(_tower);
                GetComponent<Button>().interactable = canUpgrade && canAfford;

                var upgradeCost = _tower.GetUpgradeCost();
                var text = canUpgrade && upgradeCost.HasValue ? $"Upgrade ({upgradeCost.Value})" : "Upgrade";
                GetComponentInChildren<Text>().text = text;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponentInChildren<Text>().text = "Upgrade";
            }
        }

        private void SetInteractable(int money)
        {
            var canAfford = _tower != null && money >= _tower.GetUpgradeCost();
            GetComponent<Button>().interactable = canAfford;
        }
    }
}

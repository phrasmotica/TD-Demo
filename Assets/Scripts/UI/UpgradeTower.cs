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

        public TowerManager TowerManager;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Upgrade);

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;
            TowerController.OnSellSelectedTower += SetState;

            TowerManager.OnSelectedTowerChange += SetState;
        }

        private void Upgrade() => TowerController.UpgradeSelectedTower();

        private void SetState(TowerBehaviour tower)
        {
            var canUpgradeTower = CanUpgradeTower(tower);
            GetComponent<Button>().interactable = canUpgradeTower;

            var upgradeCost = GetUpgradeCost(tower);
            var text = canUpgradeTower && upgradeCost.HasValue ? $"Upgrade ({upgradeCost.Value})" : "Upgrade";
            GetComponentInChildren<Text>().text = text;
        }

        private bool CanUpgradeTower(TowerBehaviour tower)
        {
            var canUpgrade = tower != null && tower.CanBeUpgraded();
            return canUpgrade && MoneyController.CanAfford(GetUpgradeCost(tower).Value);
        }

        private int? GetUpgradeCost(TowerBehaviour tower)
        {
            return tower != null ? tower.GetUpgradeCost() : (int?) null;
        }
    }
}

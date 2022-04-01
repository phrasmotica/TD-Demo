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
            var canUpgrade = TowerController.CanUpgradeTower(tower);
            var canAfford = TowerController.CanAffordToUpgradeTower(tower);
            GetComponent<Button>().interactable = canUpgrade && canAfford;

            var upgradeCost = TowerController.GetUpgradeCost(tower);
            var text = canUpgrade && upgradeCost.HasValue ? $"Upgrade ({upgradeCost.Value})" : "Upgrade";
            GetComponentInChildren<Text>().text = text;
        }
    }
}

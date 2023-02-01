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

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(TowerController.UpgradeSelectedTower);

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;
            TowerController.OnChangeSelectedTower += SetState;
            TowerController.OnSellSelectedTower += SetState;
        }

        private void SetState(TowerBehaviour tower)
        {
            if (tower != null)
            {
                var canUpgrade = tower.CanBeUpgraded();
                var canAfford = MoneyController.CanAffordToUpgrade(tower);
                GetComponent<Button>().interactable = canUpgrade && canAfford;

                var upgradeCost = tower.GetUpgradeCost();
                var text = canUpgrade && upgradeCost.HasValue ? $"Upgrade ({upgradeCost.Value})" : "Upgrade";
                GetComponentInChildren<Text>().text = text;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponentInChildren<Text>().text = "Upgrade";
            }
        }
    }
}

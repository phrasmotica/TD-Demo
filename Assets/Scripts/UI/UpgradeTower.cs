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
                var (canUpgrade, cost) = _tower.GetUpgradeInfo();

                GetComponent<Button>().interactable = canUpgrade && MoneyController.CanAfford(cost);
                GetComponentInChildren<Text>().text = canUpgrade ? $"Upgrade ({cost})" : "Upgrade";
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponentInChildren<Text>().text = "Upgrade";
            }
        }

        private void SetInteractable(int money)
        {
            if (_tower != null)
            {
                var (_, cost) = _tower.GetUpgradeInfo();
                GetComponent<Button>().interactable = money >= cost;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}

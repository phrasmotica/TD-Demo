using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class CreateTower : MonoBehaviour
    {
        public GameObject TowerPrefab;

        public TowerController TowerController;

        public MoneyController MoneyController;

        private int TowerPrice => TowerPrefab.GetComponent<TowerBehaviour>().Price;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(CreateNewTower);

            MoneyController.OnMoneyChange += money => SetInteractable(money >= TowerPrice);
        }

        private void CreateNewTower() => TowerController.CreateNewTower(TowerPrefab, TowerPrice);

        private void SetInteractable(bool canAfford)
        {
            GetComponent<Button>().interactable = canAfford;
        }
    }
}

using System;
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

        private int TowerPrice => TowerPrefab.GetComponent<TowerBehaviour>().Price;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(CreateNewTower);
        }

        private void CreateNewTower() => TowerController.CreateNewTower(TowerPrefab, TowerPrice);

        public void SetInteractable(Func<int, bool> canAfford)
        {
            GetComponent<Button>().interactable = canAfford(TowerPrice);
        }
    }
}

﻿using TDDemo.Assets.Scripts.Controller;
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

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(CreateNewTower);

            MoneyController.OnMoneyChange += SetInteractable;
        }

        private void CreateNewTower() => TowerController.CreateNewTower(TowerPrefab);

        private void SetInteractable(int money)
        {
            var canAfford = money >= GetTowerPrice();
            GetComponent<Button>().interactable = canAfford;
        }

        private int GetTowerPrice() => TowerPrefab.GetComponent<TowerBehaviour>().GetPrice();
    }
}

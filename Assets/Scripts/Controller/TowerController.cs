﻿using System;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class TowerController : MonoBehaviour
    {
        public TowerManager TowerManager;

        public MoneyController MoneyController;

        /// <summary>
        /// The fraction of its price that a tower should sell for.
        /// </summary>
        [Range(0.5f, 1)]
        public float SellFraction;

        /// <summary>
        /// The new tower object.
        /// </summary>
        private GameObject _newTowerObj;

        /// <summary>
        /// Gets or sets whether we're positioning a newly-created tower.
        /// </summary>
        public bool IsPositioningNewTower { get; set; }

        /// <summary>
        /// Delegate to fire when the selected tower starts upgrading.
        /// </summary>
        public event Action<TowerBehaviour> OnStartUpgradeSelectedTower;

        /// <summary>
        /// Delegate to fire when the selected tower finishes upgrading.
        /// </summary>
        public event Action<TowerBehaviour> OnFinishUpgradeSelectedTower;

        /// <summary>
        /// Delegate to fire when the selected tower is sold.
        /// </summary>
        public event Action<TowerBehaviour> OnSellSelectedTower;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && IsPositioningNewTower)
            {
                CancelCreateTower();
            }

            if (Input.GetKeyUp(KeyCode.Delete))
            {
                SellSelectedTower();
            }
        }

        public void CreateNewTower(GameObject towerPrefab)
        {
            // only create if we can afford the tower
            var tower = towerPrefab.GetComponent<TowerBehaviour>();
            if (MoneyController.CanAfford(tower.GetPrice()))
            {
                TowerManager.DeselectCurrentTower();

                IsPositioningNewTower = true;

                _newTowerObj = Instantiate(towerPrefab);
                var newTower = _newTowerObj.GetComponent<TowerBehaviour>();
                newTower.TowerController = this;
            }
        }

        public void PlaceTower(TowerBehaviour newTower)
        {
            MoneyController.AddMoney(-newTower.GetPrice());

            TowerManager.Add(newTower);
            newTower.TowerManager = TowerManager;
            newTower.OnStartUpgrade += OnStartUpgradeSelectedTower;
            newTower.OnFinishUpgrade += OnFinishUpgradeSelectedTower;

            _newTowerObj = null;
            IsPositioningNewTower = false;
        }

        public void UpgradeSelectedTower()
        {
            var selectedTower = TowerManager.GetSelectedTower();
            if (CanUpgradeTower(selectedTower) && CanAffordToUpgradeTower(selectedTower))
            {
                MoneyController.AddMoney(-selectedTower.GetUpgradeCost().Value);
                selectedTower.DoUpgrade();
            }
        }

        public void SellSelectedTower()
        {
            var selectedTower = TowerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                var sellPrice = GetSellPrice(selectedTower);
                MoneyController.AddMoney(sellPrice.Value);

                TowerManager.Remove(selectedTower);

                OnSellSelectedTower?.Invoke(selectedTower);
            }
        }

        /// <summary>
        /// Cancels tower creation.
        /// </summary>
        public void CancelCreateTower()
        {
            if (_newTowerObj != null)
            {
                Destroy(_newTowerObj);
                _newTowerObj = null;
            }

            IsPositioningNewTower = false;
        }

        public void ResetTowers()
        {
            var towers = GameObject.FindGameObjectsWithTag(Tags.Tower);
            foreach (var t in towers)
            {
                Destroy(t);
            }
        }

        public bool CanUpgradeTower(TowerBehaviour tower)
        {
            return tower != null && tower.CanBeUpgraded();
        }

        public bool CanAffordToUpgradeTower(TowerBehaviour tower)
        {
            var cost = GetUpgradeCost(tower);
            return cost.HasValue && MoneyController.CanAfford(cost.Value);
        }

        public int? GetUpgradeCost(TowerBehaviour tower)
        {
            return tower != null ? tower.GetUpgradeCost() : null;
        }

        public int? GetSellPrice(TowerBehaviour tower)
        {
            if (tower == null)
            {
                return null;
            }

            var adjustedSellPrice = (int) (tower.TotalValue * SellFraction);
            return Mathf.Max(adjustedSellPrice, 1);
        }
    }
}

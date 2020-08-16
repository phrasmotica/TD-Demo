using Assets.Scripts.Controller;
using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CreateTower : MonoBehaviour
    {
        /// <summary>
        /// The tower prefab.
        /// </summary>
        public GameObject TowerPrefab;

        /// <summary>
        /// Gets the price of the tower.
        /// </summary>
        private int TowerPrice => TowerPrefab.GetComponent<Tower>().Price;

        /// <summary>
        /// The upgrade tower script.
        /// </summary>
        public UpgradeTower UpgradeTower;

        /// <summary>
        /// The sell tower script.
        /// </summary>
        public SellTower SellTower;

        /// <summary>
        /// The money controller.
        /// </summary>
        public MoneyController MoneyController;

        /// <summary>
        /// Set click listener.
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(CreateTowerObj);
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void CreateTowerObj()
        {
            using (var logger = new MethodLogger(nameof(CreateTower)))
            {
                // only create if we can afford the tower
                if (MoneyController.CanAfford(TowerPrice))
                {
                    logger.Log("Creating tower");
                    var towerObj = Instantiate(TowerPrefab);
                    var tower = towerObj.GetComponent<Tower>();
                    tower.UpgradeTower = UpgradeTower;
                    tower.SellTower = SellTower;
                    tower.OnPlace = (price) => MoneyController.AddMoney(-price);
                }
                else
                {
                    logger.Log("Cannot afford tower!");
                }
            }
        }

        /// <summary>
        /// Sets whether this button is interactable.
        /// </summary>
        public void SetInteractable(int money)
        {
            GetComponent<Button>().interactable = money >= TowerPrice;
        }
    }
}

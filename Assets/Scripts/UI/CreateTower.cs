using Assets.Scripts.Controller;
using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CreateTower : BaseBehaviour
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
        /// The new tower object.
        /// </summary>
        private GameObject newTowerObj;

        /// <summary>
        /// The money controller.
        /// </summary>
        public MoneyController MoneyController;

        /// <summary>
        /// The tower controller.
        /// </summary>
        private TowerController towerController;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(CreateTowerObj);

            towerController = GetComponentInParent<TowerController>();
            logger = new MethodLogger(nameof(CreateTower));
        }

        /// <summary>
        /// Listen for key presses.
        /// </summary>
        private void Update()
        {
            // escape to cancel new tower creation
            if (towerController.IsPositioningNewTower && Input.GetKeyUp(KeyCode.Escape))
            {
                Cancel();
            }
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void CreateTowerObj()
        {
            // only create if we can afford the tower
            if (MoneyController.CanAfford(TowerPrice))
            {
                logger.Log("Creating tower");
                newTowerObj = Instantiate(TowerPrefab);
                var newTower = newTowerObj.GetComponent<Tower>();

                var towerController = GetComponentInParent<TowerController>();
                towerController.IsPositioningNewTower = true;
                newTower.TowerController = towerController;

                newTower.OnPlace = (price) =>
                {
                    MoneyController.AddMoney(-price);
                    newTowerObj = null;
                    towerController.AddTower(newTower);
                    towerController.IsPositioningNewTower = false;
                };
            }
            else
            {
                logger.Log("Cannot afford tower!");
            }
        }

        /// <summary>
        /// Cancels tower creation.
        /// </summary>
        public void Cancel()
        {
            logger.Log("Cancelling tower creation");

            if (newTowerObj != null)
            {
                Destroy(newTowerObj);
                newTowerObj = null;
            }

            towerController.IsPositioningNewTower = false;
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

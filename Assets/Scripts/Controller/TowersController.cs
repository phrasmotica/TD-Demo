using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class TowersController : MonoBehaviour
    {
        /// <summary>
        /// The tower prefab.
        /// </summary>
        public GameObject TowerPrefab;

        /// <summary>
        /// The sell tower button.
        /// </summary>
        public Button SellTowerButton;

        /// <summary>
        /// The fraction of its price that a tower should sell for.
        /// </summary>
        [Range(0.5f, 1)]
        public float SellFraction;

        /// <summary>
        /// The selected tower.
        /// </summary>
        public Tower SelectedTower
        {
            get
            {
                return selectedTower;
            }
            set
            {
                selectedTower = value;
                SellTowerButton.interactable = selectedTower != null;
            }
        }
        private Tower selectedTower;

        /// <summary>
        /// The money controller.
        /// </summary>
        private MoneyController MoneyController;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            MoneyController = gameObject.GetComponent<MoneyController>();
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void CreateTower()
        {
            using (var logger = new MethodLogger(nameof(TowersController)))
            {
                // only create if we can afford the tower
                if (MoneyController.CanAfford(TowerPrefab.GetComponent<Tower>().Price))
                {
                    logger.Log("Creating tower");
                    var towerObj = Instantiate(TowerPrefab);
                    var tower = towerObj.GetComponent<Tower>();
                    tower.TowersController = this;
                    tower.OnPlace = (price) => MoneyController.AddMoney(-price);
                }
                else
                {
                    logger.Log("Cannot afford tower!");
                }
            }
        }

        /// <summary>
        /// Sells the given tower.
        /// </summary>
        public void SellTower()
        {
            using (var logger = new MethodLogger(nameof(TowersController)))
            {
                if (SelectedTower != null)
                {
                    var sellPrice = (int) (SelectedTower.Price * SellFraction);
                    logger.Log($"Selling tower for {sellPrice}");

                    MoneyController.AddMoney(sellPrice);
                    Destroy(SelectedTower.gameObject);
                    SelectedTower = null;
                }
                else
                {
                    logger.LogError("Select a tower first!");
                }
            }
        }
    }
}

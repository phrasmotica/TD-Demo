using Assets.Scripts.Controller;
using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        /// <summary>
        /// The tower prefab.
        /// </summary>
        public GameObject TowerPrefab;

        /// <summary>
        /// The money controller.
        /// </summary>
        public MoneyController MoneyController;

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
                SetInteractable();
            }
        }
        private Tower selectedTower;

        /// <summary>
        /// Set click listener.
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SellTowerObj);
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void SellTowerObj()
        {
            using (var logger = new MethodLogger(nameof(SellTower)))
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

        /// <summary>
        /// Sets whether this button is interactable.
        /// </summary>
        public void SetInteractable()
        {
            GetComponent<Button>().interactable = SelectedTower != null;
        }
    }
}

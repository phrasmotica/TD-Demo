using Assets.Scripts.Controller;
using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SellTower : BaseBehaviour
    {
        /// <summary>
        /// The money controller.
        /// </summary>
        public MoneyController MoneyController;

        /// <summary>
        /// The tower controller.
        /// </summary>
        private TowerController towerController;

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
        /// Gets whether the tower can be sold.
        /// </summary>
        private bool CanSellTower => SelectedTower != null && SelectedTower.IsSelected;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SellTowerObj);

            towerController = GetComponentInParent<TowerController>();
            logger = new MethodLogger(nameof(SellTower));
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void SellTowerObj()
        {
            if (SelectedTower != null)
            {
                var sellPrice = (int) (SelectedTower.TotalValue * SellFraction);
                logger.Log($"Selling tower for {sellPrice}");

                MoneyController.AddMoney(sellPrice);
                Destroy(SelectedTower.gameObject);
                SelectedTower.DetachFromUI();

                towerController.RemoveTower(SelectedTower);
            }
            else
            {
                logger.LogError("Select a tower first!");
            }
        }

        /// <summary>
        /// Sets whether this button is interactable.
        /// </summary>
        public void SetInteractable()
        {
            GetComponent<Button>().interactable = CanSellTower;
        }
    }
}

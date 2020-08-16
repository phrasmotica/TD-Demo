using Assets.Scripts.Controller;
using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UpgradeTower : MonoBehaviour
    {
        /// <summary>
        /// The money controller.
        /// </summary>
        public MoneyController MoneyController;

        /// <summary>
        /// Gets the upgrade price of the tower.
        /// </summary>
        private int TowerUpgradePrice => SelectedTower.UpgradePrice;

        /// <summary>
        /// Gets whether the tower can be upgraded.
        /// </summary>
        private bool CanUpgradeTower => SelectedTower != null && MoneyController.CanAfford(TowerUpgradePrice);

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
            GetComponent<Button>().onClick.AddListener(UpgradeTowerObj);
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void UpgradeTowerObj()
        {
            using (var logger = new MethodLogger(nameof(UpgradeTower)))
            {
                if (CanUpgradeTower)
                {
                    logger.Log($"Upgrading tower for {TowerUpgradePrice}");

                    MoneyController.AddMoney(-TowerUpgradePrice);
                    SelectedTower.transform.Find("upgrade1").gameObject.SetActive(true);
                }
                else
                {
                    logger.LogError("Cannot upgrade tower!");
                }
            }
        }

        /// <summary>
        /// Sets whether this button is interactable.
        /// </summary>
        public void SetInteractable()
        {
            GetComponent<Button>().interactable = CanUpgradeTower;
        }
    }
}

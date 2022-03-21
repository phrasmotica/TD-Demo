using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTower : BaseBehaviour
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
        private bool CanUpgradeTower => SelectedTower != null && SelectedTower.IsSelected && SelectedTower.CanUpgrade && MoneyController.CanAfford(TowerUpgradePrice);

        /// <summary>
        /// The selected tower.
        /// </summary>
        public Tower SelectedTower
        {
            get
            {
                return _selectedTower;
            }
            set
            {
                _selectedTower = value;
                SetState();
            }
        }

        private Tower _selectedTower;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(UpgradeTowerObj);

            logger = new MethodLogger(nameof(UpgradeTower));
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void UpgradeTowerObj()
        {
            if (CanUpgradeTower)
            {
                logger.Log($"Upgrading tower for {TowerUpgradePrice}");

                MoneyController.AddMoney(-TowerUpgradePrice);
                SelectedTower.DoUpgrade();
            }
            else
            {
                logger.LogError("Cannot upgrade tower!");
            }
        }

        /// <summary>
        /// Sets this button's state.
        /// </summary>
        public void SetState()
        {
            GetComponent<Button>().interactable = CanUpgradeTower;

            if (SelectedTower != null)
            {
                GetComponentInChildren<Text>().text = $"Upgrade ({SelectedTower.UpgradePrice})";
            }
            else
            {
                GetComponentInChildren<Text>().text = "Upgrade";
            }
        }
    }
}

using Assets.Scripts.Towers;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TowerController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the selected tower.
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
                upgradeTower.SelectedTower = selectedTower;
                sellTower.SelectedTower = selectedTower;
            }
        }
        private Tower selectedTower;

        /// <summary>
        /// The upgrade tower script.
        /// </summary>
        private UpgradeTower upgradeTower;

        /// <summary>
        /// The sell tower script.
        /// </summary>
        private SellTower sellTower;

        /// <summary>
        /// Get references to child UI scripts.
        /// </summary>
        private void Start()
        {
            upgradeTower = GetComponentInChildren<UpgradeTower>();
            sellTower = GetComponentInChildren<SellTower>();
        }

        /// <summary>
        /// Refreshes the child UI scripts.
        /// </summary>
        public void Refresh()
        {
            upgradeTower.SetInteractable();
        }
    }
}

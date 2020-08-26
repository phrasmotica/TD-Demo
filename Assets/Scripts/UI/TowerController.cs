using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TowerController : BaseBehaviour
    {
        /// <summary>
        /// Gets or sets the towers.
        /// </summary>
        private List<Tower> towers;

        /// <summary>
        /// Gets the number of towers.
        /// </summary>
        public int TowerCount => towers.Count;

        /// <summary>
        /// Gets the selected tower.
        /// </summary>
        public Tower SelectedTower => towers.Any() ? towers[selectedTowerIndex] : null;

        /// <summary>
        /// Gets whether no tower is selected.
        /// </summary>
        public bool TowerAlreadySelected => SelectedTower != null && SelectedTower.IsSelected;

        /// <summary>
        /// Gets or sets the index of the selected tower.
        /// </summary>
        public int SelectedTowerIndex
        {
            get
            {
                return selectedTowerIndex;
            }
            set
            {
                if (value != selectedTowerIndex)
                {
                    // record whether the last tower was selected
                    var wasSelected = SelectedTower.IsSelected;
                    SelectedTower.IsSelected = false;

                    selectedTowerIndex = value;

                    // show it appropriately selected
                    SelectedTower.IsSelected = wasSelected;
                }

                SetChildren();
                RefreshChildren();
            }
        }
        private int selectedTowerIndex;

        /// <summary>
        /// Gets or sets whether we're positioning a newly-created tower.
        /// </summary>
        public bool IsPositioningNewTower { get; set; }

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

            towers = new List<Tower>();

            logger = new MethodLogger(nameof(TowerController));
        }

        /// <summary>
        /// Listen for tower cycle keypresses.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Deselect();
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (SelectedTower.IsSelected)
                {
                    SelectPreviousTower();
                }
                else
                {
                    SelectCurrentTower();
                }
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (SelectedTower.IsSelected)
                {
                    SelectNextTower();
                }
                else
                {
                    SelectCurrentTower();
                }
            }
        }

        /// <summary>
        /// Adds the given tower.
        /// </summary>
        public void AddTower(Tower tower)
        {
            towers.Add(tower);
            logger.Log($"Added tower with instance ID {tower.GetInstanceID()}, new tower count: {TowerCount}");
        }

        /// <summary>
        /// Removes the given tower.
        /// </summary>
        public void RemoveTower(Tower tower)
        {
            SelectedTowerIndex = Math.Max(0, SelectedTowerIndex - 1);

            towers.Remove(tower);
            logger.Log($"Removed tower with instance ID {tower.GetInstanceID()}, new tower count: {TowerCount}");
        }

        /// <summary>
        /// Selects the current tower.
        /// </summary>
        public void SelectCurrentTower()
        {
            SelectedTower.IsSelected = true;
            SetChildren();
            RefreshChildren();
            logger.Log("Selected current tower");
        }

        /// <summary>
        /// Selects the given tower.
        /// </summary>
        public void Select(Tower tower)
        {
            SelectedTowerIndex = towers.IndexOf(tower);
            logger.Log($"Selected a tower, index {SelectedTowerIndex}");
        }

        /// <summary>
        /// Selects the previous tower.
        /// </summary>
        public void SelectPreviousTower()
        {
            var newIndex = SelectedTowerIndex - 1;
            if (newIndex < 0)
            {
                // cannot use % on negative number
                newIndex += TowerCount;
            }

            SelectedTowerIndex = newIndex;
            logger.Log($"Selected previous tower, index {SelectedTowerIndex}");
        }

        /// <summary>
        /// Selects the next tower.
        /// </summary>
        public void SelectNextTower()
        {
            SelectedTowerIndex = (SelectedTowerIndex + 1) % TowerCount;
            logger.Log($"Selected next tower, index {SelectedTowerIndex}");
        }

        /// <summary>
        /// Deselected the selected tower.
        /// </summary>
        public void Deselect()
        {
            SelectedTower.IsSelected = false;
            RefreshChildren();
        }

        /// <summary>
        /// Sets the selected tower in the child UI scripts.
        /// </summary>
        public void SetChildren()
        {
            upgradeTower.SelectedTower = SelectedTower;
            sellTower.SelectedTower = SelectedTower;
        }

        /// <summary>
        /// Refreshes the child UI scripts.
        /// </summary>
        public void RefreshChildren()
        {
            upgradeTower.SetInteractable();
            sellTower.SetInteractable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class TowerManager : MonoBehaviour
    {
        private List<TowerBehaviour> _towers;

        private int _selectedTowerIndex;

        public event Action<TowerBehaviour> OnSelectedTowerChange;

        private void Start()
        {
            _towers = new List<TowerBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DeselectCurrentTower();
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (GetSelectedTower() != null)
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
                if (GetSelectedTower() != null)
                {
                    SelectNextTower();
                }
                else
                {
                    SelectCurrentTower();
                }
            }
        }

        public void Add(TowerBehaviour tower)
        {
            _towers.Add(tower);
        }

        public void Remove(TowerBehaviour tower)
        {
            _towers.Remove(tower);
            Destroy(tower.gameObject);
        }

        public void Select(TowerBehaviour tower)
        {
            if (tower != null)
            {
                SetSelectedTowerIndex(_towers.IndexOf(tower));
            }
        }

        /// <summary>
        /// Selects the previous tower.
        /// </summary>
        public void SelectPreviousTower()
        {
            var newIndex = _selectedTowerIndex - 1;
            if (newIndex < 0)
            {
                // cannot use % on negative number
                newIndex += _towers.Count;
            }

            SetSelectedTowerIndex(newIndex);
        }

        /// <summary>
        /// Selects the next tower.
        /// </summary>
        public void SelectNextTower()
        {
            SetSelectedTowerIndex((_selectedTowerIndex + 1) % _towers.Count);
        }

        private void SetSelectedTowerIndex(int index)
        {
            if (_selectedTowerIndex != index)
            {
                DeselectCurrentTower();

                _selectedTowerIndex = index;
            }

            SelectCurrentTower();
        }

        /// <summary>
        /// Selects the current tower.
        /// </summary>
        private void SelectCurrentTower()
        {
            var selectedTower = GetCurrentTower();
            if (selectedTower != null)
            {
                selectedTower.SetIsSelected(true);
            }

            OnSelectedTowerChange?.Invoke(selectedTower);
        }

        /// <summary>
        /// Deselects the current tower.
        /// </summary>
        public void DeselectCurrentTower()
        {
            var selectedTower = GetCurrentTower();
            if (selectedTower != null)
            {
                selectedTower.SetIsSelected(false);
            }

            OnSelectedTowerChange?.Invoke(null);
        }

        /// <summary>
        /// Gets the selected tower.
        /// </summary>
        public TowerBehaviour GetSelectedTower()
        {
            var tower = GetCurrentTower();
            return tower != null && tower.IsSelected ? tower : null;
        }

        private TowerBehaviour GetCurrentTower()
        {
            if (!_towers.Any())
            {
                return null;
            }

            if (_selectedTowerIndex < 0 || _selectedTowerIndex >= _towers.Count)
            {
                return null;
            }

            return _towers[_selectedTowerIndex];
        }

        public void ClearTowers()
        {
            foreach (var t in _towers)
            {
                Destroy(t.gameObject);
            }
        }
    }
}

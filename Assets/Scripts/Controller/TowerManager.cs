using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Towers;

namespace TDDemo.Assets.Scripts.Controller
{
    public class TowerManager
    {
        private int _selectedTowerIndex;

        public List<TowerBehaviour> Towers { get; }

        public TowerManager() 
        {
            Towers = new List<TowerBehaviour>();
        }

        public void Add(TowerBehaviour tower)
        {
            Towers.Add(tower);
        }

        public void Remove(TowerBehaviour tower)
        {
            Towers.Remove(tower);
        }

        public TowerBehaviour Select(TowerBehaviour tower)
        {
            if (tower != null)
            {
                return SetSelectedTowerIndex(Towers.IndexOf(tower));
            }

            return null;
        }

        public TowerBehaviour WakeOrSelectPrevious()
        {
            if (GetSelectedTower() != null)
            {
                return SelectPreviousTower();
            }
            
            return SelectCurrentTower();
        }

        public TowerBehaviour WakeOrSelectNext()
        {
            if (GetSelectedTower() != null)
            {
                return SelectNextTower();
            }
            
            return SelectCurrentTower();
        }

        public void DeselectCurrentTower()
        {
            var selectedTower = GetCurrentTower();
            if (selectedTower != null)
            {
                selectedTower.SetIsSelected(false);
            }
        }

        public TowerBehaviour GetSelectedTower()
        {
            var tower = GetCurrentTower();
            return tower != null && tower.IsSelected ? tower : null;
        }

        private TowerBehaviour SelectPreviousTower()
        {
            var newIndex = _selectedTowerIndex - 1;
            if (newIndex < 0)
            {
                // cannot use % on negative number
                newIndex += Towers.Count;
            }

            return SetSelectedTowerIndex(newIndex);
        }

        private TowerBehaviour SelectNextTower()
        {
            return SetSelectedTowerIndex((_selectedTowerIndex + 1) % Towers.Count);
        }

        private TowerBehaviour SetSelectedTowerIndex(int index)
        {
            if (_selectedTowerIndex != index)
            {
                DeselectCurrentTower();

                _selectedTowerIndex = index;
            }

            return SelectCurrentTower();
        }

        private TowerBehaviour SelectCurrentTower()
        {
            var selectedTower = GetCurrentTower();
            if (selectedTower != null)
            {
                selectedTower.SetIsSelected(true);
            }

            return selectedTower;
        }

        private TowerBehaviour GetCurrentTower()
        {
            if (!Towers.Any())
            {
                return null;
            }

            if (_selectedTowerIndex < 0 || _selectedTowerIndex >= Towers.Count)
            {
                return null;
            }

            return Towers[_selectedTowerIndex];
        }
    }
}

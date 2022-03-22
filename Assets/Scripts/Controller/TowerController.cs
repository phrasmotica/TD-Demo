using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Controller
{
    public class TowerController : MonoBehaviour
    {
        public UpgradeTower UpgradeTower;

        public SellTower SellTower;

        private TowerManager _towerManager;

        /// <summary>
        /// The fraction of its price that a tower should sell for.
        /// </summary>
        [Range(0.5f, 1)]
        public float SellFraction;

        private MoneyController _moneyController;

        /// <summary>
        /// The new tower object.
        /// </summary>
        private GameObject _newTowerObj;

        /// <summary>
        /// Gets or sets whether we're positioning a newly-created tower.
        /// </summary>
        public bool IsPositioningNewTower { get; set; }

        /// <summary>
        /// Get references to child UI scripts.
        /// </summary>
        private void Start()
        {
            _towerManager = GetComponent<TowerManager>();
            _moneyController = GetComponent<MoneyController>();

            SellTower.GetComponent<Button>().onClick.AddListener(SellSelectedTower);
            UpgradeTower.GetComponent<Button>().onClick.AddListener(UpgradeSelectedTower);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && IsPositioningNewTower)
            {
                CancelCreateTower();
            }

            if (Input.GetKeyUp(KeyCode.Delete))
            {
                SellSelectedTower();
            }
        }

        public void CreateNewTower(GameObject towerPrefab, int price)
        {
            // only create if we can afford the tower
            if (_moneyController.CanAfford(price))
            {
                _towerManager.DeselectCurrentTower();

                IsPositioningNewTower = true;

                _newTowerObj = Instantiate(towerPrefab);
                var newTower = _newTowerObj.GetComponent<Tower>();
                newTower.TowerController = this;
            }
        }

        public void PlaceTower(Tower newTower)
        {
            _moneyController.AddMoney(-newTower.Price);

            _towerManager.Add(newTower);
            newTower.TowerManager = _towerManager;

            _newTowerObj = null;
            IsPositioningNewTower = false;
        }

        public void UpgradeSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (CanUpgradeTower(selectedTower))
            {
                _moneyController.AddMoney(-selectedTower.UpgradePrice);
                selectedTower.DoUpgrade();

                Refresh();
            }
        }

        public void SellSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                var sellPrice = GetSellPrice(selectedTower);
                _moneyController.AddMoney(sellPrice.Value);

                _towerManager.Remove(selectedTower);

                Refresh();
            }
        }

        /// <summary>
        /// Refreshes the child UI scripts.
        /// </summary>
        public void Refresh()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            UpgradeTower.SetState(CanUpgradeTower(selectedTower), GetUpgradePrice(selectedTower));
            SellTower.SetState(selectedTower != null, GetSellPrice(selectedTower));
        }

        /// <summary>
        /// Cancels tower creation.
        /// </summary>
        public void CancelCreateTower()
        {
            if (_newTowerObj != null)
            {
                Destroy(_newTowerObj);
                _newTowerObj = null;
            }

            IsPositioningNewTower = false;
        }

        private bool CanUpgradeTower(Tower tower)
        {
            var canUpgrade = tower != null && tower.CanBeUpgraded();
            return canUpgrade && _moneyController.CanAfford(tower.UpgradePrice);
        }

        private int? GetUpgradePrice(Tower tower)
        {
            return tower != null ? tower.UpgradePrice : (int?) null;
        }

        private int? GetSellPrice(Tower tower)
        {
            if (tower == null)
            {
                return null;
            }

            var adjustedSellPrice = (int) (tower.TotalValue * SellFraction);
            return Mathf.Max(adjustedSellPrice, 1);
        }
    }
}

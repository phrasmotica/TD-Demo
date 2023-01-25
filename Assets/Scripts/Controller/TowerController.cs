using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class TowerController : MonoBehaviour
    {
        public LivesController LivesController;

        public MoneyController MoneyController;

        public WavesController WavesController;

        public List<CreateTower> CreateTowers;

        public UpgradeTower UpgradeTower;

        public SellTower SellTower;

        public GameOver GameOver;

        private TowerManager _towerManager;

        private TowerBehaviour _newTower;

        public event UnityAction OnCreateNewTower;

        public event UnityAction<TowerBehaviour> OnPlaceTower;

        public event UnityAction<TowerBehaviour> OnStartUpgradeSelectedTower;

        public event UnityAction<TowerBehaviour> OnFinishUpgradeSelectedTower;

        public event UnityAction<TowerBehaviour> OnChangeSelectedTower;

        public event UnityAction<TowerBehaviour> OnSellSelectedTower;

        private void Start()
        {
            _towerManager = new TowerManager();

            OnCreateNewTower += () =>
            {
                _towerManager.DeselectCurrentTower();
                OnChangeSelectedTower?.Invoke(null);
            };

            OnPlaceTower += _towerManager.Add;

            OnSellSelectedTower += tower =>
            {
                _towerManager.Remove(tower);
                Destroy(tower.gameObject);
            };

            LivesController.OnEndGame += CancelCreateTower;

            foreach (var createTower in CreateTowers)
            {
                createTower.OnCreate += CreateNewTower;
            }

            UpgradeTower.OnUpgrade += UpgradeSelectedTower;

            SellTower.OnSell += SellSelectedTower;

            GameOver.OnRestart += () =>
            {
                _towerManager.DeselectCurrentTower();
                OnChangeSelectedTower?.Invoke(null);

                foreach (var t in _towerManager.Towers)
                {
                    Destroy(t.gameObject);
                }

                _towerManager.Towers.Clear();
            };
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _towerManager.DeselectCurrentTower();
                OnChangeSelectedTower?.Invoke(null);

                if (_newTower != null && _newTower.IsPositioning)
                {
                    CancelCreateTower();
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                var tower = _towerManager.WakeOrSelectPrevious();
                OnChangeSelectedTower?.Invoke(tower);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                var tower = _towerManager.WakeOrSelectNext();
                OnChangeSelectedTower?.Invoke(tower);
            }

            if (Input.GetKeyUp(KeyCode.Delete))
            {
                SellSelectedTower();
            }
        }

        private void CreateNewTower(GameObject towerPrefab)
        {
            // only create if we can afford the tower
            var tower = towerPrefab.GetComponent<TowerBehaviour>();
            if (MoneyController.CanAffordToBuy(tower))
            {
                OnCreateNewTower();

                var newTowerObj = Instantiate(towerPrefab);

                _newTower = newTowerObj.GetComponent<TowerBehaviour>();
                _newTower.OnPlace += () => PlaceTower(_newTower);
            }
        }

        private void PlaceTower(TowerBehaviour newTower)
        {
            OnPlaceTower(newTower);

            WavesController.OnEnemiesChange += newTower.SetEnemies;

            newTower.OnClicked += () =>
            {
                _towerManager.Select(newTower);
                OnChangeSelectedTower?.Invoke(newTower);
            };

            newTower.OnFinishUpgrade += () => OnFinishUpgradeSelectedTower(newTower);

            _newTower = null;
        }

        private void UpgradeSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null && selectedTower.CanBeUpgraded() && MoneyController.CanAffordToUpgrade(selectedTower))
            {
                OnStartUpgradeSelectedTower?.Invoke(selectedTower);
            }
        }

        private void SellSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                _towerManager.DeselectCurrentTower();
                OnChangeSelectedTower?.Invoke(null);

                OnSellSelectedTower(selectedTower);
            }
        }

        private void CancelCreateTower()
        {
            if (_newTower != null)
            {
                Destroy(_newTower.gameObject);
                _newTower = null;
            }
        }
    }
}

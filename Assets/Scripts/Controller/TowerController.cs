using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.UI;
using TDDemo.Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class TowerController : MonoBehaviour
    {
        public GameObject Canvas;

        public GameObject RewardTextPrefab;

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

        public event UnityAction<TowerBehaviour> OnLevelChangeSelectedTower;

        public event UnityAction<TowerBehaviour> OnSetTargetMethodTower;

        public event UnityAction<TowerBehaviour, int> OnXpChangeTower;

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

            OnXpChangeTower += (tower, amount) =>
            {
                if (amount != 0)
                {
                    var textPos = tower.transform.position + new Vector3(0, 0.2f);
                    var text = Instantiate(RewardTextPrefab, textPos, Quaternion.identity, Canvas.transform);
                    var textComponent = text.GetComponent<TextMeshProUGUI>();

                    textComponent.text = amount < 0 ? $"{amount}" : $"+{amount}";
                    textComponent.color = amount < 0 ? ColourHelper.XpLoss : ColourHelper.Xp;
                }
            };

            OnSellSelectedTower += tower =>
            {
                if (tower != null)
                {
                    _towerManager.Remove(tower);
                    Destroy(tower.gameObject);
                }
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

            newTower.OnFinishUpgrade += () =>
            {
                if (newTower.IsSelected)
                {
                    OnFinishUpgradeSelectedTower(newTower);
                }
            };

            newTower.OnLevelChange += () =>
            {
                if (newTower.IsSelected)
                {
                    OnLevelChangeSelectedTower(newTower);
                }
            };

            newTower.OnSetTargetMethod += method =>
            {
                if (newTower.IsSelected)
                {
                    OnSetTargetMethodTower?.Invoke(newTower);
                }
            };

            newTower.OnXpChange += amount => OnXpChangeTower(newTower, amount);

            _newTower = null;
        }

        public void SetTargetingSelectedTower(TargetMethod method)
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                selectedTower.SetTargetMethod(method);
            }
        }

        // TODO: make this public and remove UpgradeTower as a dependency, similar to SetTargetingSelectedTower
        private void UpgradeSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null && selectedTower.CanBeUpgraded() && MoneyController.CanAffordToUpgrade(selectedTower))
            {
                OnStartUpgradeSelectedTower?.Invoke(selectedTower);
            }
        }

        // TODO: make this public and remove SellTower as a dependency, similar to SetTargetingSelectedTower
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

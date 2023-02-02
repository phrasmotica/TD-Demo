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

        public GameOver GameOver;

        private TowerManager _towerManager;

        private TowerBehaviour _newTower;

        public event UnityAction<TowerBehaviour> OnPlaceTower;

        public event UnityAction<TowerBehaviour> OnStartUpgradeSelectedTower;

        public event UnityAction<TowerBehaviour> OnFinishUpgradeSelectedTower;

        public event UnityAction<TowerBehaviour> OnChangeSelectedTower;

        public event UnityAction<TowerBehaviour> OnLevelChangeSelectedTower;

        public event UnityAction<TowerBehaviour> OnSetTargetMethodTower;

        public event UnityAction<TowerBehaviour, int> OnKillCountChangeTower;

        public event UnityAction<TowerBehaviour, int> OnXpChangeTower;

        public event UnityAction<TowerBehaviour> OnSellSelectedTower;

        private void Start()
        {
            _towerManager = new TowerManager();

            LivesController.OnEndGame += CancelCreateTower;

            GameOver.OnRestart += () =>
            {
                Deselect();

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
                Deselect();

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

        public void CreateNewTower(GameObject towerPrefab)
        {
            // only create if we can afford the tower
            var tower = towerPrefab.GetComponent<TowerBehaviour>();
            if (MoneyController.CanAffordToBuy(tower))
            {
                Deselect();

                _newTower = Instantiate(towerPrefab).GetComponent<TowerBehaviour>();
                _newTower.OnPlace += () => PlaceTower(_newTower);
            }
        }

        private void PlaceTower(TowerBehaviour newTower)
        {
            _towerManager.Add(newTower);

            OnPlaceTower?.Invoke(newTower);

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
                    OnFinishUpgradeSelectedTower?.Invoke(newTower);
                }
            };

            newTower.OnLevelChange += () =>
            {
                if (newTower.IsSelected)
                {
                    OnLevelChangeSelectedTower?.Invoke(newTower);
                }
            };

            newTower.OnSetTargetMethod += method =>
            {
                if (newTower.IsSelected)
                {
                    OnSetTargetMethodTower?.Invoke(newTower);
                }
            };

            newTower.OnXpChange += amount =>
            {
                CreateEphemeralXpText(newTower, amount);
                OnXpChangeTower?.Invoke(newTower, amount);
            };

            newTower.OnKillCountChange += kills =>
            {
                OnKillCountChangeTower?.Invoke(newTower, kills);
            };

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

        public void UpgradeSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                var (canUpgrade, cost) = selectedTower.GetUpgradeInfo();
                if (canUpgrade && MoneyController.CanAfford(cost))
                {
                    OnStartUpgradeSelectedTower?.Invoke(selectedTower);
                }
            }
        }

        public void SellSelectedTower()
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                Deselect();

                if (selectedTower != null)
                {
                    _towerManager.Remove(selectedTower);
                    Destroy(selectedTower.gameObject);
                }

                OnSellSelectedTower?.Invoke(selectedTower);
            }
        }

        private void Deselect()
        {
            _towerManager.DeselectCurrentTower();
            OnChangeSelectedTower?.Invoke(null);
        }

        private void CreateEphemeralXpText(TowerBehaviour tower, int amount)
        {
            if (amount != 0)
            {
                var textPos = tower.transform.position + new Vector3(0, 0.2f);
                var text = Instantiate(RewardTextPrefab, textPos, Quaternion.identity, Canvas.transform);
                var textComponent = text.GetComponent<TextMeshProUGUI>();

                textComponent.text = amount < 0 ? $"{amount}" : $"+{amount}";
                textComponent.color = amount < 0 ? ColourHelper.XpLoss : ColourHelper.Xp;
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

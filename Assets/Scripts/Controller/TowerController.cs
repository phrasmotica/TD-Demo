using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Actions;
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

        public BankManager Bank;

        public WavesController WavesController;

        private TowerManager _towerManager;

        private TowerBehaviour _newTower;

        public UnityEvent<TowerBehaviour> OnPlaceTower;

        public UnityEvent<TowerBehaviour> OnStartUpgradeSelectedTower;

        public UnityEvent<TowerBehaviour> OnFinishUpgradeSelectedTower;

        public UnityEvent<TowerBehaviour> OnChangeSelectedTower;

        public UnityEvent<TowerBehaviour> OnLevelChangeSelectedTower;

        public UnityEvent<TowerBehaviour> OnSetTargetMethodTower;

        public UnityEvent<TowerBehaviour, int> OnKillCountChangeTower;

        public UnityEvent<TowerBehaviour, int> OnXpChangeTower;

        public UnityEvent<TowerBehaviour> OnSellSelectedTower;

        private void Start()
        {
            _towerManager = new TowerManager();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Deselect();

                if (_newTower != null && _newTower.IsPositioning())
                {
                    CancelCreateTower();
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                var tower = _towerManager.WakeOrSelectPrevious();
                OnChangeSelectedTower.Invoke(tower);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                var tower = _towerManager.WakeOrSelectNext();
                OnChangeSelectedTower.Invoke(tower);
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

            var canBuy = Bank.CanAffordToBuy(tower) != PurchaseMethod.None;
            if (canBuy)
            {
                Deselect();

                _newTower = Instantiate(towerPrefab).GetComponent<TowerBehaviour>();

                // cannot be set in the editor because the new tower is not known ahead of time
                _newTower.OnPlace.AddListener(() => PlaceTower(_newTower));
            }
        }

        private void PlaceTower(TowerBehaviour newTower)
        {
            // TODO: these events are a mess! Things get routed through multiple objects and with
            // references to the objects whose events are being added to... fix them!

            _towerManager.Add(newTower);
            OnPlaceTower.Invoke(newTower);

            // cannot be set in the editor because the new tower is not known ahead of time?
            // Maybe there's a way we can make it work... perhaps creating a new EnemiesController
            // that holds the list of enemies in a static field.
            WavesController.OnEnemiesChange.AddListener(newTower.SetEnemies);

            // cannot be set in the editor because the new tower is not known ahead of time
            newTower.OnClicked.AddListener(tower =>
            {
                _towerManager.Select(tower);
                OnChangeSelectedTower.Invoke(tower);
            });

            // cannot be set in the editor because the new tower is not known ahead of time
            newTower.OnFinishUpgrade.AddListener(tower =>
            {
                if (tower.IsSelected)
                {
                    OnFinishUpgradeSelectedTower.Invoke(tower);
                }
            });

            // cannot be set in the editor because the new tower is not known ahead of time
            newTower.OnLevelChange.AddListener(tower =>
            {
                if (tower.IsSelected)
                {
                    OnLevelChangeSelectedTower.Invoke(tower);
                }
            });

            // cannot be set in the editor because the new tower is not known ahead of time
            newTower.OnSetTargetMethod.AddListener((tower, method) =>
            {
                if (tower.IsSelected)
                {
                    OnSetTargetMethodTower.Invoke(tower);
                }
            });

            // cannot be set in the editor because the new tower is not known ahead of time
            newTower.OnXpChange.AddListener((tower, amount) =>
            {
                CreateEphemeralXpText(tower, amount);
                OnXpChangeTower.Invoke(tower, amount);
            });

            // cannot be set in the editor because the new tower is not known ahead of time
            newTower.OnKillCountChange.AddListener((tower, kills) =>
            {
                OnKillCountChangeTower.Invoke(tower, kills);
            });

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

        public void UpgradeSelectedTower(PurchaseMethod purchaseMethod)
        {
            var selectedTower = _towerManager.GetSelectedTower();
            if (selectedTower != null)
            {
                var (canUpgrade, price) = selectedTower.GetUpgradeInfo();

                if (canUpgrade && Bank.TryBuyVia(price, purchaseMethod))
                {
                    selectedTower.DoUpgrade();

                    OnStartUpgradeSelectedTower.Invoke(selectedTower);
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

                OnSellSelectedTower.Invoke(selectedTower);
            }
        }

        private void Deselect()
        {
            _towerManager.DeselectCurrentTower();
            OnChangeSelectedTower.Invoke(null);
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

        public void CancelCreateTower()
        {
            if (_newTower != null)
            {
                Destroy(_newTower.gameObject);
                _newTower = null;
            }
        }

        public void ResetAll()
        {
            Deselect();

            foreach (var t in _towerManager.Towers)
            {
                Destroy(t.gameObject);
            }

            _towerManager.Towers.Clear();
        }
    }
}

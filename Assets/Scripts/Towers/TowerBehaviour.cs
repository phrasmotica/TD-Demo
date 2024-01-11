using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Experience;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Towers.Upgrades;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerBehaviour : BaseBehaviour, IStructure
    {
        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public string Description { get; set; }

        public SpriteRenderer SpriteRenderer;

        public Transform PedestalTransform;

        public TargetLine TargetLine;

        public TargetMethod TargetMethod;

        public GoldCalculator GoldCalculator;

        public XpCalculator XpCalculator;

        public UpgradeTree UpgradeTree;

        private Tower _tower;

        private List<GameObject> _enemies;

        private ITowerAction[] _actions;

        private StrikeProvider[] _strikes;

        private bool _isCollidingWithAnotherTower;

        private bool _isCollidingWithPathZone;

        public bool IsSelected { get; private set; }

        public int KillCount { get; private set; }

        public ExperienceContainer Experience => _tower.Experience;

        public UnityEvent<TowerBehaviour> OnMouseEnterEvent;

        public UnityEvent<TowerBehaviour> OnMouseExitEvent;

        public UnityEvent<bool> OnSelected;

        public UnityEvent<TowerBehaviour> OnClicked;

        public UnityEvent<bool> OnCanBePlaced;

        public UnityEvent OnPlace;

        public UnityEvent OnReadyActions;

        public UnityEvent<ITowerAction[]> OnRefreshActions;

        public UnityEvent<StrikeProvider[]> OnRefreshStrikes;

        public UnityEvent OnStartWarmup;

        public UnityEvent OnStartUpgrade;

        public UnityEvent<TowerBehaviour, float> OnWarmupProgress;

        public UnityEvent<TowerBehaviour, float> OnUpgradeProgress;

        public UnityEvent OnFinishWarmup;

        public UnityEvent<TowerBehaviour> OnFinishUpgrade;

        public UnityEvent<TowerBehaviour> OnLevelChange;

        public UnityEvent<TowerBehaviour, int> OnKillCountChange;

        public UnityEvent<TowerBehaviour, int> OnXpChange;

        public UnityEvent<TowerBehaviour, TargetMethod> OnSetTargetMethod;

        private void Start()
        {
            _tower = new Tower(GoldCalculator, XpCalculator);
            _enemies = new();

            RefreshActions();
            RefreshStrikes();

            logger = new MethodLogger(nameof(TowerBehaviour));
        }

        private void Update()
        {
            if (_tower.IsPositioning())
            {
                transform.FollowMouse();

                if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                {
                    if (_isCollidingWithAnotherTower)
                    {
                        logger.Log("Tower collision, cannot place here");
                    }
                    else if (_isCollidingWithPathZone)
                    {
                        logger.Log("Path collision, cannot place here");
                    }
                    else
                    {
                        var mousePosition = Input.mousePosition;
                        var worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
                        logger.Log($"Placed tower at {worldPoint}");

                        OnPlace.Invoke();

                        StartCoroutine(Warmup());
                    }
                }
            }

            if (_tower.IsWarmingUp())
            {
                var progress = _tower.Warmup(Time.deltaTime);
                OnWarmupProgress.Invoke(this, progress);
            }

            if (_tower.IsUpgrading())
            {
                var progress = _tower.Upgrade(Time.deltaTime);
                OnUpgradeProgress.Invoke(this, progress);
            }

            foreach (var action in _actions)
            {
                action.Survey(_enemies);

                if (_tower.IsFiring())
                {
                    action.Act();
                }
            }
        }

        public void LookAtTarget(Enemy enemy)
        {
            if (enemy != null && _tower.IsFiring())
            {
                var direction = (enemy.transform.position - PedestalTransform.position).normalized;
                PedestalTransform.up = direction;
            }
        }

        private void OnMouseEnter() => OnMouseEnterEvent.Invoke(this);

        private void OnMouseExit() => OnMouseExitEvent.Invoke(this);

        private void OnMouseUp()
        {
            if (_tower.IsFiring() && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                logger.Log("Selected tower");
                OnClicked.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(Tags.Tower))
            {
                SetIsCollidingWithAnotherTower(true);
            }

            if (collider.gameObject.CompareTag(Tags.PathZone))
            {
                SetIsCollidingWithPathZone(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(Tags.Tower))
            {
                SetIsCollidingWithAnotherTower(false);
            }

            if (collider.gameObject.CompareTag(Tags.PathZone))
            {
                SetIsCollidingWithPathZone(false);
            }
        }

        public void SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
            OnSelected.Invoke(IsSelected);
        }

        public void SetIsCollidingWithAnotherTower(bool isColliding)
        {
            _isCollidingWithAnotherTower = isColliding;
            OnCanBePlaced.Invoke(CanBePlaced());
        }

        public void SetIsCollidingWithPathZone(bool isColliding)
        {
            _isCollidingWithPathZone = isColliding;
            OnCanBePlaced.Invoke(CanBePlaced());
        }

        private IEnumerator Warmup()
        {
            OnStartWarmup.Invoke();

            var warmupTime = UpgradeTree.GetWarmupTime();
            _tower.StartWarmingUp(warmupTime);

            SpriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower warming up for {warmupTime} seconds");
            yield return new WaitForSeconds(warmupTime);

            _tower.FinishWarmingUp();

            OnReadyActions.Invoke();
            AllowFire();

            SpriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log("Tower ready");

            OnFinishWarmup.Invoke();
        }

        public void DoUpgrade(int index) => StartCoroutine(Upgrade(index));

        private IEnumerator Upgrade(int index)
        {
            OnStartUpgrade.Invoke();

            var upgradeTime = UpgradeTree.GetUpgradeTime(index);
            _tower.StartUpgrading(upgradeTime);

            PreventFire();

            SpriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower upgrading for {upgradeTime} seconds");
            yield return new WaitForSeconds(upgradeTime);

            _tower.FinishUpgrading();
            UpgradeTree.Upgrade(index);

            OnFinishUpgrade.Invoke(this);

            RefreshActions();
            RefreshStrikes();
            AllowFire();

            var upgradedSprite = UpgradeTree.GetCurrent().Sprite;
            if (upgradedSprite != null)
            {
                SpriteRenderer.sprite = upgradedSprite;
            }

            SpriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log($"Tower upgraded, total value {GetTotalValue()}");
        }

        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        public bool CanBeUpgraded() => _tower.IsFiring() && UpgradeTree.CanUpgrade();

        public bool IsPositioning() => _tower.IsPositioning();

        public bool IsFiring() => _tower.IsFiring();

        public bool IsUpgrading() => _tower.IsUpgrading();

        public int GetTotalValue() => UpgradeTree.GetTotalValue();

        public List<GameObject> GetEnemies() => _enemies;

        public void SetEnemies(List<GameObject> enemies) => _enemies = enemies;

        public void SetTargetMethod(TargetMethod method)
        {
            TargetMethod = method;

            RefreshActions();
            RefreshStrikes();

            OnSetTargetMethod.Invoke(this, method);
        }

        public int GetPrice() => UpgradeTree.GetPrice();

        public (bool, UpgradeNode) GetUpgradeInfo(int index)
        {
            var nextLevels = UpgradeTree.GetUpgrades();
            if (nextLevels == null || !nextLevels.Any())
            {
                return (false, null);
            }

            if (index > nextLevels.Count - 1)
            {
                return (false, null);
            }

            return (_tower.IsFiring(), nextLevels[index]);
        }

        public string GetName()
        {
            var upgradedName = UpgradeTree.GetCurrent().Name;

            if (!string.IsNullOrEmpty(upgradedName))
            {
                return upgradedName;
            }

            return Name;
        }

        public float GetDamage()
        {
            var actionsWithDamage = GetStrikes<DamageEnemy>();
            return actionsWithDamage.Any() ? actionsWithDamage.Max(a => a.Amount) : 0;
        }

        public float GetFireRate()
        {
            var actionsWithFireRate = GetActions<IHasFireRate>();
            return actionsWithFireRate.Any() ? actionsWithFireRate.Max(a => a.GetFireRate()) : 0;
        }

        public void GainKill() => OnKillCountChange.Invoke(this, ++KillCount);

        public void GainXp(int baseXp)
        {
            var xp = _tower.AddXp(baseXp);
            OnXpChange.Invoke(this, xp);

            if (_tower.TryLevelUp())
            {
                OnLevelChange.Invoke(this);
            }
        }

        public int ComputeGoldReward(int baseGoldReward) => _tower.ComputeGoldReward(baseGoldReward);

        private void AllowFire()
        {
            foreach (var a in _actions)
            {
                a.CanAct = true;
            }
        }

        private void PreventFire()
        {
            foreach (var a in _actions)
            {
                a.CanAct = false;
            }
        }

        private void RefreshActions()
        {
            _actions = GetComponentsInChildren<ITowerAction>();

            foreach (var a in _actions)
            {
                a.TargetMethod = TargetMethod;
            }

            OnRefreshActions.Invoke(_actions);
        }

        public IEnumerable<T> GetActions<T>() => _actions.OfType<T>().Cast<T>();

        private void RefreshStrikes()
        {
            _strikes = GetComponentsInChildren<StrikeProvider>();

            foreach (var s in _strikes)
            {
                s.TargetMethod = TargetMethod;
            }

            OnRefreshStrikes.Invoke(_strikes);
        }

        private IEnumerable<T> GetStrikes<T>() => _strikes.OfType<T>().Cast<T>();
    }

    public enum GoldCalculator
    {
        Default,
        LevelLinear,
    }

    public enum XpCalculator
    {
        Default,
        LevelLinear,
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerBehaviour : BaseBehaviour
    {
        public string Name;

        public string Description;

        public TargetMethod TargetMethod;

        public GoldCalculator GoldCalculator;

        public XpCalculator XpCalculator;

        public List<TowerLevel> Levels;

        private Tower _tower;

        private List<GameObject> _enemies;

        public bool IsSelected { get; private set; }

        private SpriteRenderer _spriteRenderer;

        private ITowerAction[] _actions;

        private StrikeProvider[] _strikes;

        private bool _isCollidingWithAnotherTower;

        private bool _isCollidingWithPathZone;

        public Scripts.Experience.Experience Experience => _tower.Experience;

        public int TotalValue => _tower.GetTotalValue();

        public bool IsPositioning => _tower.IsPositioning();

        public event UnityAction OnMouseEnterEvent;

        public event UnityAction OnMouseExitEvent;

        public event UnityAction<bool> OnSelected;

        public event UnityAction OnClicked;

        public event UnityAction<bool> OnCanBePlaced;

        public event UnityAction OnPlace;

        public event UnityAction<ITowerAction[]> OnAccumulateActions;

        public event UnityAction<StrikeProvider[]> OnAccumulateStrikes;

        public event UnityAction<float> OnWarmupProgress;

        public event UnityAction<float> OnUpgradeProgress;

        public event UnityAction OnFinishUpgrade;

        public event UnityAction OnLevelChange;

        public event UnityAction<int> OnXpChange;

        public event UnityAction<TargetMethod> OnSetTargetMethod;

        private void Start()
        {
            _tower = new Tower(Levels, GoldCalculator, XpCalculator);
            _enemies = new();

            _spriteRenderer = GetComponent<SpriteRenderer>();

            // TODO: accumulate actions (or do something else) to establish
            // the tower's range here, so that the range is drawn correctly
            // before the tower has finished warming up

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

                        OnPlace?.Invoke();

                        StartCoroutine(Warmup());
                    }
                }
            }

            if (_tower.IsWarmingUp())
            {
                var progress = _tower.Warmup(Time.deltaTime);
                OnWarmupProgress?.Invoke(progress);
            }

            if (_tower.IsUpgrading())
            {
                var progress = _tower.Upgrade(Time.deltaTime);
                OnUpgradeProgress?.Invoke(progress);
            }

            if (_tower.IsFiring())
            {
                foreach (var action in _actions)
                {
                    action.Act(_enemies);
                }
            }
        }

        private void OnMouseEnter() => OnMouseEnterEvent?.Invoke();

        private void OnMouseExit() => OnMouseExitEvent?.Invoke();

        private void OnMouseUp()
        {
            if (_tower.IsFiring() && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                logger.Log("Selected tower");
                OnClicked?.Invoke();
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
            OnSelected?.Invoke(isSelected);
        }

        public void SetIsCollidingWithAnotherTower(bool isColliding)
        {
            _isCollidingWithAnotherTower = isColliding;
            OnCanBePlaced?.Invoke(CanBePlaced());
        }

        public void SetIsCollidingWithPathZone(bool isColliding)
        {
            _isCollidingWithPathZone = isColliding;
            OnCanBePlaced?.Invoke(CanBePlaced());
        }

        private IEnumerator Warmup()
        {
            var warmupTime = _tower.StartWarmingUp();

            _spriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower warming up for {warmupTime} seconds");
            yield return new WaitForSeconds(warmupTime);

            _tower.FinishWarmingUp();

            AccumulateActions();
            AccumulateStrikes();
            AllowFire();

            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log("Tower ready");
        }

        public void DoUpgrade() => StartCoroutine(Upgrade());

        private IEnumerator Upgrade()
        {
            var upgradeTime = _tower.StartUpgrading();
            PreventFire();

            _spriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower upgrading for {upgradeTime} seconds");
            yield return new WaitForSeconds(upgradeTime);

            var newLevel = _tower.FinishUpgrading();

            AccumulateActions();
            AccumulateStrikes();
            AllowFire();

            _spriteRenderer.sprite = newLevel.GetComponent<SpriteRenderer>().sprite;
            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log($"Tower upgraded, total value {TotalValue}");

            OnFinishUpgrade?.Invoke();
        }

        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        public bool CanBeUpgraded() => _tower.CanBeUpgraded();

        public bool IsWarmingUp() => _tower.IsWarmingUp();

        public bool IsUpgrading() => _tower.IsUpgrading();

        public void SetEnemies(List<GameObject> enemies) => _enemies = enemies;

        public void SetTargetMethod(TargetMethod method)
        {
            TargetMethod = method;

            AccumulateActions();
            AccumulateStrikes();

            OnSetTargetMethod?.Invoke(method);
        }

        public int GetPrice() => Levels.First().Price;

        public int? GetUpgradeCost() => _tower.GetUpgradeCost();

        public float GetDamage()
        {
            var actionsWithDamage = GetStrikes<DamageEnemy>();
            return actionsWithDamage.Any() ? actionsWithDamage.Max(a => a.Amount) : 0;
        }

        public int GetRange()
        {
            var actionsWithRange = GetActions<IHasRange>();
            return actionsWithRange.Any() ? actionsWithRange.Max(a => a.GetRange()) : 0;
        }

        public float GetFireRate()
        {
            var actionsWithFireRate = GetActions<IHasFireRate>();
            return actionsWithFireRate.Any() ? actionsWithFireRate.Max(a => a.GetFireRate()) : 0;
        }

        public void GainXp(int baseXp)
        {
            var xp = _tower.AddXp(baseXp);
            OnXpChange?.Invoke(xp);

            if (_tower.TryLevelUp())
            {
                OnLevelChange?.Invoke();
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

        private void AccumulateActions()
        {
            _actions = GetComponentsInChildren<ITowerAction>();

            foreach (var a in _actions)
            {
                a.TargetMethod = TargetMethod;
                (a as MonoBehaviour).enabled = true;
            }

            OnAccumulateActions?.Invoke(_actions);
        }

        private IEnumerable<T> GetActions<T>() => _actions.OfType<T>().Cast<T>();

        private void AccumulateStrikes()
        {
            _strikes = GetComponentsInChildren<StrikeProvider>();

            foreach (var s in _strikes)
            {
                s.TargetMethod = TargetMethod;
            }

            OnAccumulateStrikes?.Invoke(_strikes);
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

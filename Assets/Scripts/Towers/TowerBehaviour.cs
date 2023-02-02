using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Experience;
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

        private SpriteRenderer _spriteRenderer;

        private ITowerAction[] _actions;

        private StrikeProvider[] _strikes;

        private bool _isCollidingWithAnotherTower;

        private bool _isCollidingWithPathZone;

        public bool IsSelected { get; private set; }

        public ExperienceContainer Experience => _tower.Experience;

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
            _tower = new Tower(GoldCalculator, XpCalculator);
            _enemies = new();

            _spriteRenderer = GetComponent<SpriteRenderer>();

            AccumulateActions();
            AccumulateStrikes();

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
            var warmupTime = GetWarmupTime();
            _tower.StartWarmingUp(warmupTime);

            _spriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower warming up for {warmupTime} seconds");
            yield return new WaitForSeconds(warmupTime);

            _tower.FinishWarmingUp();

            ReadyActions();

            AccumulateActions();
            AccumulateStrikes();
            AllowFire();

            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log("Tower ready");
        }

        public void DoUpgrade() => StartCoroutine(Upgrade());

        private IEnumerator Upgrade()
        {
            var upgradeTime = GetUpgradeTime();
            _tower.StartUpgrading(upgradeTime);

            PreventFire();

            _spriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower upgrading for {upgradeTime} seconds");
            yield return new WaitForSeconds(upgradeTime);

            var newLevel = _tower.FinishUpgrading();

            // make sure only the current level object is active
            for (var i = 0; i < Levels.Count; i++)
            {
                var level = Levels[i];
                level.gameObject.SetActive(i == newLevel);
            }

            AccumulateActions();
            AccumulateStrikes();
            AllowFire();

            _spriteRenderer.sprite = Levels[newLevel].GetComponent<SpriteRenderer>().sprite;
            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log($"Tower upgraded, total value {GetTotalValue()}");

            OnFinishUpgrade?.Invoke();
        }

        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        public bool CanBeUpgraded() => _tower.IsFiring() && _tower.UpgradeLevel < Levels.Count - 1;

        public bool IsWarmingUp() => _tower.IsWarmingUp();

        public bool IsUpgrading() => _tower.IsUpgrading();

        public int GetTotalValue() => Levels.Take(_tower.UpgradeLevel + 1).Sum(l => l.Price);

        private float GetWarmupTime() => Levels.First().Time;

        private float GetUpgradeTime()
        {
            var nextLevel = Levels[_tower.UpgradeLevel + 1];
            return nextLevel.Time;
        }

        public void SetEnemies(List<GameObject> enemies) => _enemies = enemies;

        public void SetTargetMethod(TargetMethod method)
        {
            TargetMethod = method;

            AccumulateActions();
            AccumulateStrikes();

            OnSetTargetMethod?.Invoke(method);
        }

        public int GetPrice() => Levels.First().Price;

        public (bool, int) GetUpgradeInfo()
        {
            var isMaxLevel = _tower.UpgradeLevel >= Levels.Count - 1;
            if (isMaxLevel)
            {
                return (false, 0);
            }

            var nextLevel = Levels[_tower.UpgradeLevel + 1];
            return (_tower.IsFiring(), nextLevel.Price);
        }

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

        private void ReadyActions()
        {
            _actions = GetComponentsInChildren<ITowerAction>();

            foreach (var a in _actions)
            {
                a.Ready();
            }
        }

        // TODO: rename to RefreshActions() or similar
        private void AccumulateActions()
        {
            _actions = GetComponentsInChildren<ITowerAction>();

            foreach (var a in _actions)
            {
                a.TargetMethod = TargetMethod;
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

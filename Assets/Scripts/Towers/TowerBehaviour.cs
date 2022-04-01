using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerBehaviour : BaseBehaviour
    {
        public string Name;

        public List<TowerLevel> Levels;

        public GameObject SelectionObj;

        public Range Range;

        private Tower _tower;

        public TowerController TowerController { get; set; }

        public TowerManager TowerManager { get; set; }

        public bool IsSelected { get; private set; }

        public int TotalValue => _tower.GetTotalValue();

        private SpriteRenderer _spriteRenderer;

        private ITowerAction[] _actions;

        /// <summary>
        /// Whether this tower is colliding with another tower.
        /// </summary>
        private bool _isCollidingWithAnotherTower;

        /// <summary>
        /// Whether this tower is colliding with a path zone.
        /// </summary>
        private bool _isCollidingWithPathZone;

        public event Action<float> OnWarmupProgress;

        public event Action<TowerBehaviour> OnStartUpgrade;

        public event Action<float> OnUpgradeProgress;

        public event Action<TowerBehaviour> OnFinishUpgrade;

        private void Start()
        {
            _tower = new Tower(Levels);

            _spriteRenderer = GetComponent<SpriteRenderer>();

            AccumulateActions();

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

                        TowerController.PlaceTower(this);
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
                var enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy);

                foreach (var action in _actions)
                {
                    action.Act(enemies);
                }
            }
        }

        private void OnMouseEnter()
        {
            if (!TowerController.IsPositioningNewTower && !IsSelected)
            {
                Range.gameObject.SetActive(true);
            }
        }

        private void OnMouseExit()
        {
            if (!TowerController.IsPositioningNewTower && !IsSelected)
            {
                Range.gameObject.SetActive(false);
            }
        }

        private void OnMouseUp()
        {
            if (_tower.IsFiring() && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                logger.Log("Selected tower");
                TowerManager.Select(this);
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
            SelectionObj.SetActive(isSelected);
            Range.gameObject.SetActive(isSelected);
        }

        public void SetIsCollidingWithAnotherTower(bool isColliding)
        {
            _isCollidingWithAnotherTower = isColliding;
            Range.SetTowerCanBePlaced(CanBePlaced());
        }

        public void SetIsCollidingWithPathZone(bool isColliding)
        {
            _isCollidingWithPathZone = isColliding;
            Range.SetTowerCanBePlaced(CanBePlaced());
        }

        /// <summary>
        /// Make the tower warm up before being ready to fire.
        /// </summary>
        private IEnumerator Warmup()
        {
            var warmupTime = _tower.StartWarmingUp();

            _spriteRenderer.color = ColourHelper.HalfOpacity;

            logger.Log($"Tower warming up for {warmupTime} seconds");
            yield return new WaitForSeconds(warmupTime);

            _tower.FinishWarmingUp();
            AllowFire();

            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log("Tower ready");
        }

        public void DoUpgrade() => StartCoroutine(Upgrade());

        /// <summary>
        /// Upgrades the tower to the next level.
        /// </summary>
        private IEnumerator Upgrade()
        {
            var upgradeTime = _tower.StartUpgrading();
            PreventFire();

            _spriteRenderer.color = ColourHelper.HalfOpacity;

            OnStartUpgrade?.Invoke(this);

            logger.Log($"Tower upgrading for {upgradeTime} seconds");
            yield return new WaitForSeconds(upgradeTime);

            var newLevel = _tower.FinishUpgrading();

            AccumulateActions();
            AllowFire();

            _spriteRenderer.sprite = newLevel.GetComponent<SpriteRenderer>().sprite;
            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log($"Tower upgraded, total value {TotalValue}");

            OnFinishUpgrade?.Invoke(this);
        }

        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        public bool CanBeUpgraded() => _tower.CanBeUpgraded();

        public bool IsWarmingUp() => _tower.IsWarmingUp();

        public bool IsUpgrading() => _tower.IsUpgrading();

        public int GetPrice() => Levels.First().Price;

        public int? GetUpgradeCost() => _tower.GetUpgradeCost();

        public int GetDamage()
        {
            var actionsWithDamage = GetActions<IHasDamage>();
            return actionsWithDamage.Any() ? actionsWithDamage.Max(a => a.Damage) : 0;
        }

        public int GetRange()
        {
            var actionsWithRange = GetActions<IHasRange>();
            return actionsWithRange.Any() ? actionsWithRange.Max(a => a.Range) : 0;
        }

        public float GetFireRate()
        {
            var actionsWithFireRate = GetActions<IHasFireRate>();
            return actionsWithFireRate.Any() ? actionsWithFireRate.Max(a => a.FireRate) : 0;
        }

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

            Range.SetRange(GetRange());
        }

        private IEnumerable<T> GetActions<T>() => _actions.OfType<T>().Cast<T>();
    }
}

using System;
using System.Collections;
using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerBehaviour : BaseBehaviour
    {
        [Range(0, 5)]
        public int Price;

        [Range(0, 3)]
        public int UpgradePrice;

        [Range(0, 5)]
        public float WarmupTime;

        [Range(0, 2)]
        public float UpgradeTime;

        private Tower _tower;

        public GameObject RangePrefab;

        public TowerController TowerController { get; set; }

        public TowerManager TowerManager { get; set; }

        public bool IsSelected { get; private set; }

        public float WarmupProgress => _tower.WarmupProgress;

        public float UpgradeProgress => _tower.UpgradeProgress;

        public int TotalValue => _tower.TotalValue;

        /// <summary>
        /// The tower's initial Z position.
        /// </summary>
        private float _initialZPos;

        /// <summary>
        /// The tower selection object.
        /// </summary>
        private GameObject _selectionObj;

        /// <summary>
        /// The tower range object.
        /// </summary>
        private Range _range;

        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// The base shoot projectile script.
        /// </summary>
        private ShootProjectile _baseShootProjectile;

        /// <summary>
        /// Gets or sets whether this tower is colliding with another tower.
        /// </summary>
        private bool _isCollidingWithAnotherTower;

        /// <summary>
        /// Gets or sets whether this tower is colliding with a path zone.
        /// </summary>
        private bool _isCollidingWithPathZone;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            _tower = new Tower(Price, UpgradePrice, WarmupTime, UpgradeTime, GetMaxUpgradeLevel());

            _initialZPos = transform.position.z;

            _selectionObj = transform.Find("selection").gameObject;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            _baseShootProjectile = GetComponent<ShootProjectile>();

            _range = transform.Find("range").GetComponent<Range>();
            _range.SetRange(_baseShootProjectile.Range);

            logger = new MethodLogger(nameof(TowerBehaviour));
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            if (_tower.IsPositioning())
            {
                var mousePosition = Input.mousePosition;
                var worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = new Vector3(worldPoint.x, worldPoint.y, _initialZPos);

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
                        logger.Log($"Placed tower at {worldPoint}");
                        TowerController.PlaceTower(this);
                        DoWarmup();
                    }
                }
            }

            if (_tower.IsWarmingUp())
            {
                _tower.Warmup(Time.deltaTime);
            }

            if (_tower.IsUpgrading())
            {
                _tower.Upgrade(Time.deltaTime);
            }
        }

        /// <summary>
        /// Mouse is over the tower so draw the range.
        /// </summary>
        private void OnMouseEnter()
        {
            if (!TowerController.IsPositioningNewTower && !IsSelected)
            {
                logger.Log("Showing range of unselected tower");
                _range.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Mouse is no longer over the tower so hide the range.
        /// </summary>
        private void OnMouseExit()
        {
            if (!TowerController.IsPositioningNewTower && !IsSelected)
            {
                logger.Log("Hiding range of unselected tower");
                _range.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Set this as the selected tower when clicked.
        /// </summary>
        private void OnMouseUp()
        {
            if (_tower.IsFiring() && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                logger.Log("Selected tower");
                TowerManager.Select(this);
            }
        }

        /// <summary>
        /// Set tower collision flag if necessary.
        /// </summary>
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

        /// <summary>
        /// Clear tower collision flag if necessary.
        /// </summary>
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
            _selectionObj.SetActive(isSelected);
            _range.gameObject.SetActive(isSelected);
        }

        public void SetIsCollidingWithAnotherTower(bool isColliding)
        {
            _isCollidingWithAnotherTower = isColliding;
            _range.SetTowerCanBePlaced(CanBePlaced());
        }

        public void SetIsCollidingWithPathZone(bool isColliding)
        {
            _isCollidingWithPathZone = isColliding;
            _range.SetTowerCanBePlaced(CanBePlaced());
        }

        /// <summary>
        /// Starts the coroutine to make the tower warm up.
        /// </summary>
        private void DoWarmup()
        {
            _spriteRenderer.color = ColourHelper.HalfOpacity;
            StartCoroutine(Warmup());
        }

        /// <summary>
        /// Make the tower warm up before being ready to fire.
        /// </summary>
        private IEnumerator Warmup()
        {
            _tower.StartWarmingUp();

            logger.Log($"Tower warming up for {WarmupTime} seconds");
            yield return new WaitForSeconds(WarmupTime);

            _tower.FinishWarmingUp();

            logger.Log($"Tower ready");
            _spriteRenderer.color = ColourHelper.FullOpacity;
        }

        /// <summary>
        /// Starts the coroutine to upgrade the tower.
        /// </summary>
        public void DoUpgrade()
        {
            _spriteRenderer.color = ColourHelper.HalfOpacity;
            StartCoroutine(Upgrade());
        }

        /// <summary>
        /// Upgrades the tower to the next level.
        /// </summary>
        private IEnumerator Upgrade()
        {
            _tower.StartUpgrading();
            TowerController.Refresh();

            logger.Log($"Tower upgrading for {UpgradeTime} seconds");
            yield return new WaitForSeconds(UpgradeTime);

            var newUpgradeLevel = _tower.FinishUpgrading();

            logger.Log($"Tower upgraded to level {newUpgradeLevel}, total value {_tower.TotalValue}");

            // enable only the relevant upgrade object
            foreach (Transform child in transform)
            {
                if (child.CompareTag(Tags.TowerUpgrade))
                {
                    var name = child.gameObject.name;
                    child.gameObject.SetActive(name.EndsWith($"{newUpgradeLevel}", StringComparison.OrdinalIgnoreCase));

                    var childSprite = child.GetComponent<SpriteRenderer>();
                    _spriteRenderer.sprite = childSprite.sprite;
                    childSprite.enabled = false;

                    _range.SetRange(child.GetComponent<ShootProjectile>().Range);
                }
            }

            _baseShootProjectile.enabled = _tower.IsBaseLevel();

            _spriteRenderer.color = ColourHelper.FullOpacity;

            TowerController.Refresh();
        }

        private int GetMaxUpgradeLevel() => transform.GetChildCountWithTag(Tags.TowerUpgrade);

        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        public bool CanBeUpgraded() => _tower.CanBeUpgraded();

        public bool IsWarmingUp() => _tower.IsWarmingUp();

        public bool IsUpgrading() => _tower.IsUpgrading();

        public bool IsFiring() => _tower.IsFiring();
    }
}

using System;
using System.Collections;
using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Tower : BaseBehaviour
    {
        /// <summary>
        /// The range prefab.
        /// </summary>
        public GameObject RangePrefab;

        /// <summary>
        /// The tower's state.
        /// </summary>
        public TowerState State;

        public TowerController TowerController { get; set; }

        public TowerManager TowerManager { get; set; }

        /// <summary>
        /// The tower's price.
        /// </summary>
        [Range(0, 5)]
        public int Price;

        /// <summary>
        /// The tower's price.
        /// </summary>
        [Range(0, 3)]
        public int UpgradePrice;

        /// <summary>
        /// The time taken in seconds for this tower to warm up.
        /// </summary>
        [Range(0, 5)]
        public int WarmupTime;

        /// <summary>
        /// The time taken in seconds for this tower to upgrade.
        /// </summary>
        [Range(0, 2)]
        public int UpgradeTime;

        /// <summary>
        /// Gets or sets this tower's value, i.e. the price it will be sold for.
        /// </summary>
        public int TotalValue { get; set; }

        public bool IsSelected { get; private set; }

        /// <summary>
        /// The time in seconds since this tower was created.
        /// </summary>
        private float _age;

        /// <summary>
        /// The time in seconds since this tower's upgrade started.
        /// </summary>
        private float _upgradeAge;

        /// <summary>
        /// Gets the progress of this tower's warmup process.
        /// </summary>
        public float WarmupProgress => _age / WarmupTime;

        /// <summary>
        /// Gets the progress of this tower's upgrade process.
        /// </summary>
        public float UpgradeProgress => _upgradeAge / UpgradeTime;

        /// <summary>
        /// Gets whether the tower can fire.
        /// </summary>
        public bool CanFire => State == TowerState.Firing;

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
        /// The upgrade level.
        /// </summary>
        private int _upgradeLevel;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            _initialZPos = transform.position.z;
            State = TowerState.Positioning;

            _selectionObj = transform.Find("selection").gameObject;

            _range = Instantiate(RangePrefab, transform).GetComponent<Range>();

            _spriteRenderer = GetComponent<SpriteRenderer>();

            _baseShootProjectile = GetComponent<ShootProjectile>();
            _range.RangeToDraw = _baseShootProjectile.Range;

            logger = new MethodLogger(nameof(Tower));
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            if (State == TowerState.Positioning)
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
                        TotalValue += Price;
                        DoWarmup();
                    }
                }
            }

            if (State == TowerState.Warmup)
            {
                _age += Time.deltaTime;
            }

            if (State == TowerState.Upgrading)
            {
                _upgradeAge += Time.deltaTime;
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
            if (State == TowerState.Firing && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                logger.Log($"Selected tower");
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
            State = TowerState.Warmup;
            _spriteRenderer.color = ColourHelper.HalfOpacity;
            StartCoroutine(Warmup());
        }

        /// <summary>
        /// Make the tower warm up before being ready to fire.
        /// </summary>
        private IEnumerator Warmup()
        {
            logger.Log($"Tower warming up for {WarmupTime} seconds");
            yield return new WaitForSeconds(WarmupTime);

            logger.Log($"Tower ready");
            _spriteRenderer.color = ColourHelper.FullOpacity;
            State = TowerState.Firing;
        }

        /// <summary>
        /// Starts the coroutine to upgrade the tower.
        /// </summary>
        public void DoUpgrade()
        {
            State = TowerState.Upgrading;
            _spriteRenderer.color = ColourHelper.HalfOpacity;
            TowerController.Refresh();
            StartCoroutine(Upgrade());
        }

        /// <summary>
        /// Upgrades the tower to the next level.
        /// </summary>
        private IEnumerator Upgrade()
        {
            logger.Log($"Tower upgrading for {UpgradeTime} seconds");
            yield return new WaitForSeconds(UpgradeTime);

            TotalValue += UpgradePrice;
            var newUpgradeLevel = ++_upgradeLevel;

            logger.Log($"Tower upgraded to level {newUpgradeLevel}, total value {TotalValue}");

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

                    _range.RangeToDraw = child.GetComponent<ShootProjectile>().Range;
                    _range.DrawRange();
                }
            }

            _baseShootProjectile.enabled = _upgradeLevel <= 0;

            _spriteRenderer.color = ColourHelper.FullOpacity;
            State = TowerState.Firing;
            _upgradeAge = 0;

            TowerController.Refresh();
        }

        /// <summary>
        /// Removes this tower from the UI.
        /// </summary>
        public void Remove()
        {
            TowerManager.Remove(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// Returns the maximum upgrade level for this tower.
        /// </summary>
        private int GetMaxUpgradeLevel() => transform.GetChildCountWithTag(Tags.TowerUpgrade);

        /// <summary>
        /// Returns whether the tower can be places.
        /// </summary>
        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        /// <summary>
        /// Returns whether the tower can be upgraded.
        /// </summary>
        public bool CanBeUpgraded() => State == TowerState.Firing && _upgradeLevel < GetMaxUpgradeLevel();
    }
}

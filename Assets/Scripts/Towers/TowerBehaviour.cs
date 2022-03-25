using System.Collections;
using System.Linq;
using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerBehaviour : BaseBehaviour
    {
        [Range(0, 5)]
        public int Price;

        [Range(0, 5)]
        public float WarmupTime;

        private Tower _tower;

        public TowerController TowerController { get; set; }

        public TowerManager TowerManager { get; set; }

        public bool IsSelected { get; private set; }

        public float WarmupProgress => _tower.WarmupProgress;

        public float UpgradeProgress => _tower.UpgradeProgress;

        public int TotalValue => _tower.GetTotalValue();

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

        private ShootProjectile _shootProjectile;

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
            var levels = transform.GetComponentsInChildren<TowerLevel>().ToList();
            _tower = new Tower(Price, WarmupTime, levels);

            _initialZPos = transform.position.z;

            _selectionObj = transform.Find("selection").gameObject;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            var baseLevel = levels.First();

            _shootProjectile = GetComponent<ShootProjectile>();
            _shootProjectile.Level = baseLevel;

            _range = transform.Find("range").GetComponent<Range>();
            _range.SetRange(baseLevel.Range);

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
            _shootProjectile.CanFire = false;

            logger.Log($"Tower warming up for {WarmupTime} seconds");
            yield return new WaitForSeconds(WarmupTime);

            _tower.FinishWarmingUp();
            _shootProjectile.CanFire = true;

            logger.Log("Tower ready");
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
            var upgradeTime = _tower.StartUpgrading();
            _shootProjectile.CanFire = false;

            TowerController.Refresh();

            logger.Log($"Tower upgrading for {upgradeTime} seconds");
            yield return new WaitForSeconds(upgradeTime);

            var newLevel = _tower.FinishUpgrading();
            _shootProjectile.CanFire = true;

            _shootProjectile.Level = newLevel;
            _range.SetRange(newLevel.Range);

            _spriteRenderer.sprite = newLevel.GetComponent<SpriteRenderer>().sprite;
            _spriteRenderer.color = ColourHelper.FullOpacity;

            logger.Log($"Tower upgraded, total value {TotalValue}");

            TowerController.Refresh();
        }

        private bool CanBePlaced() => !_isCollidingWithAnotherTower && !_isCollidingWithPathZone;

        public bool CanBeUpgraded() => _tower.CanBeUpgraded();

        public bool IsWarmingUp() => _tower.IsWarmingUp();

        public bool IsUpgrading() => _tower.IsUpgrading();

        public TowerLevel GetLevel() => _tower.GetLevel();

        public int GetUpgradeCost() => _tower.GetUpgradeCost();
    }
}

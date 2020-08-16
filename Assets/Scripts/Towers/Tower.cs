using System;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Towers
{
    public class Tower : BaseBehaviour
    {
        /// <summary>
        /// The tower's state.
        /// </summary>
        public TowerState State;

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
        /// Gets or sets this tower's value, i.e. the price it will be sold for.
        /// </summary>
        public int TotalValue { get; set; }

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
        /// The time in seconds since this tower was created.
        /// </summary>
        private float Age;

        /// <summary>
        /// The time in seconds since this tower's upgrade started.
        /// </summary>
        private float UpgradeAge;

        /// <summary>
        /// Gets the progress of this tower's warmup process.
        /// </summary>
        public float WarmupProgress => Age / WarmupTime;

        /// <summary>
        /// Gets the progress of this tower's upgrade process.
        /// </summary>
        public float UpgradeProgress => UpgradeAge / UpgradeTime;

        /// <summary>
        /// Gets whether the tower can fire.
        /// </summary>
        public bool CanFire => State == TowerState.Firing;

        /// <summary>
        /// The tower controller script.
        /// </summary>
        public TowerController TowerController { get; set; }

        /// <summary>
        /// Gets whether no tower is selected.
        /// </summary>
        private bool NoTowerSelected => TowerController.SelectedTower == null;

        /// <summary>
        /// Delegate to run on placing the tower.
        /// </summary>
        public Action<int> OnPlace { private get; set; }

        /// <summary>
        /// The tower's initial Z position.
        /// </summary>
        private float InitialZPos;

        /// <summary>
        /// The tower selection object.
        /// </summary>
        private GameObject selectionObj;

        /// <summary>
        /// The tower range object.
        /// </summary>
        private Range range;

        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The base shoot projectile script.
        /// </summary>
        private ShootProjectile baseShootProjectile;

        /// <summary>
        /// Gets or sets whether this tower is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                selectionObj.SetActive(value);
                range.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Whether this tower is colliding with another tower.
        /// </summary>
        private bool isCollidingWithAnotherTower;

        /// <summary>
        /// The upgrade level.
        /// </summary>
        private int UpgradeLevel;
        private bool isSelected;

        /// <summary>
        /// Gets the maximum upgrade level for this tower.
        /// </summary>
        private int MaxUpgradeLevel
        {
            get
            {
                var max = 0;

                foreach (Transform child in transform)
                {
                    if (child.CompareTag(Tags.TowerUpgradeTag))
                    {
                        max++;
                    }
                }

                return max;
            }
        }

        /// <summary>
        /// Gets whether the tower can be upgraded.
        /// </summary>
        public bool CanUpgrade => UpgradeLevel < MaxUpgradeLevel;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            InitialZPos = transform.position.z;
            State = TowerState.Positioning;

            selectionObj = transform.Find("selection").gameObject;
            range = transform.Find("range").GetComponent<Range>();

            spriteRenderer = GetComponent<SpriteRenderer>();

            baseShootProjectile = GetComponent<ShootProjectile>();
            range.RangeToDraw = baseShootProjectile.Range;

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
                transform.position = new Vector3(worldPoint.x, worldPoint.y, InitialZPos);

                if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                {
                    if (!isCollidingWithAnotherTower)
                    {
                        logger.Log($"Placed tower at {worldPoint}");
                        OnPlace(Price);
                        TotalValue += Price;
                        DoWarmup();
                    }
                    else
                    {
                        logger.Log("Tower collision, cannot place here");
                    }
                }
            }

            if (State == TowerState.Warmup)
            {
                Age += Time.deltaTime;
            }

            if (State == TowerState.Upgrading)
            {
                UpgradeAge += Time.deltaTime;
            }
        }

        /// <summary>
        /// Mouse is over the tower so draw the range.
        /// </summary>
        private void OnMouseEnter()
        {
            if (!IsSelected)
            {
                logger.Log("Showing range of unselected tower");
                range.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Mouse is no longer over the tower so hide the range.
        /// </summary>
        private void OnMouseExit()
        {
            if (!IsSelected)
            {
                logger.Log("Hiding range of unselected tower");
                range.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Set this as the selected tower when clicked.
        /// </summary>
        private void OnMouseUp()
        {
            if (State == TowerState.Firing)
            {
                if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                {
                    if (NoTowerSelected)
                    {
                        logger.Log($"Selected tower");
                        IsSelected = true;
                        AttachToUI();
                    }
                    else
                    {
                        logger.Log("Another tower is already selected!");
                    }
                }
            }
        }

        /// <summary>
        /// Set tower collision flag if necessary.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(Tags.TowerTag))
            {
                isCollidingWithAnotherTower = true;
            }
        }

        /// <summary>
        /// Clear tower collision flag if necessary.
        /// </summary>
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(Tags.TowerTag))
            {
                isCollidingWithAnotherTower = false;
            }
        }

        /// <summary>
        /// Starts the coroutine to make the tower warm up.
        /// </summary>
        private void DoWarmup()
        {
            State = TowerState.Warmup;
            spriteRenderer.color = ColourHelper.HalfOpacity;
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
            spriteRenderer.color = ColourHelper.FullOpacity;
            State = TowerState.Firing;
        }

        /// <summary>
        /// Starts the coroutine to upgrade the tower.
        /// </summary>
        public void DoUpgrade()
        {
            State = TowerState.Upgrading;
            spriteRenderer.color = ColourHelper.HalfOpacity;
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
            var newUpgradeLevel = ++UpgradeLevel;

            logger.Log($"Tower upgraded to level {newUpgradeLevel}, total value {TotalValue}");

            // enable only the relevant upgrade object
            foreach (Transform child in transform)
            {
                if (child.CompareTag(Tags.TowerUpgradeTag))
                {
                    var name = child.gameObject.name;
                    child.gameObject.SetActive(name.EndsWith($"{newUpgradeLevel}", StringComparison.OrdinalIgnoreCase));

                    var childSprite = child.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = childSprite.sprite;
                    childSprite.enabled = false;

                    range.RangeToDraw = child.GetComponent<ShootProjectile>().Range;
                }
            }

            baseShootProjectile.enabled = UpgradeLevel <= 0;

            spriteRenderer.color = ColourHelper.FullOpacity;
            State = TowerState.Firing;
            UpgradeAge = 0;

            TowerController.Refresh();
        }

        /// <summary>
        /// Sets references to this tower in the UI.
        /// </summary>
        public void AttachToUI()
        {
            TowerController.SelectedTower = this;
        }

        /// <summary>
        /// Removes references to this tower from the UI.
        /// </summary>
        public void DetachFromUI()
        {
            TowerController.SelectedTower = null;
        }
    }

    /// <summary>
    /// Possible states for a tower to occupy.
    /// </summary>
    public enum TowerState
    {
        Positioning,
        Warmup,
        Firing,
        Upgrading,
    }
}

using System.Linq;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    /// <summary>
    /// Script for the range sprite of a tower.
    /// </summary>
    public class Range : BaseBehaviour
    {
        public TowerBehaviour TowerBehaviour;

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        public int RangeToDraw { get; private set; }

        /// <summary>
        /// Gets or sets whether the parent tower can be placed.
        /// </summary>
        private bool _towerCanBePlaced;

        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            TowerBehaviour.OnSelected += SetShowRange;

            TowerBehaviour.OnCanBePlaced += SetTowerCanBePlaced;

            TowerBehaviour.OnAccumulateActions += actions =>
            {
                var actionsWithRange = actions.OfType<IHasRange>();
                var range = actionsWithRange.Any() ? actionsWithRange.Max(a => a.Range) : 0;
                SetRange(range);
            };

            TowerBehaviour.OnMouseEnterEvent += () =>
            {
                if (!TowerBehaviour.TowerController.IsPositioningNewTower)
                {
                    SetShowRange(true);
                }
            };

            TowerBehaviour.OnMouseExitEvent += () =>
            {
                if (!TowerBehaviour.TowerController.IsPositioningNewTower && !TowerBehaviour.IsSelected)
                {
                    SetShowRange(false);
                }
            };

            logger = new MethodLogger(nameof(Range));

            SetTowerCanBePlaced(true);
        }

        public void SetTowerCanBePlaced(bool towerCanBePlaced)
        {
            _towerCanBePlaced = towerCanBePlaced;

            if (_spriteRenderer != null)
            {
                DrawRange();
            }
        }

        public void SetRange(int range)
        {
            RangeToDraw = range;

            if (_spriteRenderer != null)
            {
                DrawRange();
            }
        }

        /// <summary>
        /// Draws the range.
        /// </summary>
        public void DrawRange()
        {
            logger.Log($"Drawing range of {RangeToDraw}");

            // set sprite colour
            _spriteRenderer.color = _towerCanBePlaced ? CanBePlacedColour : CannotBePlacedColour;

            // radius of range sprite in world space units
            var spriteSize = _spriteRenderer.sprite.bounds.size.x / 2;

            // scale required to bring sprite to size of the range.
            var scale = RangeToDraw / spriteSize;

            transform.localScale = new Vector3(scale, scale, 1);
        }

        private void SetShowRange(bool isSelected)
        {
            _spriteRenderer.enabled = isSelected;
        }

        /// <summary>
        /// Gets the sprite colour for when the tower can be placed.
        /// </summary>
        private static Color CanBePlacedColour => Color.white;

        /// <summary>
        /// Gets the sprite colour for when the tower cannot be placed.
        /// </summary>
        private static Color CannotBePlacedColour => Color.red;
    }
}

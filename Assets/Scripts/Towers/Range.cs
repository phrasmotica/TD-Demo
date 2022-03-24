using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    /// <summary>
    /// Script for the range sprite of a tower.
    /// </summary>
    public class Range : BaseBehaviour
    {
        /// <summary>
        /// Gets or sets the range to draw.
        /// </summary>
        private int _range;

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
            logger = new MethodLogger(nameof(Range));
        }

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            SetTowerCanBePlaced(true);
        }

        public void SetTowerCanBePlaced(bool towerCanBePlaced)
        {
            _towerCanBePlaced = towerCanBePlaced;
            DrawRange();
        }

        public void SetRange(int range)
        {
            _range = range;

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
            logger.Log($"Drawing range of {_range}");

            // set sprite colour
            _spriteRenderer.color = _towerCanBePlaced ? CanBePlacedColour : CannotBePlacedColour;

            // width (thus height) of sprite in world space units
            var spriteSize = _spriteRenderer.sprite.bounds.size.x;

            // scale required to bring sprite to size of the range
            var scale = _range / spriteSize;

            transform.localScale = new Vector3(scale, scale, 1);
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

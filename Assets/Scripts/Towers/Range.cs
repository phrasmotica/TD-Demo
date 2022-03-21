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
        public int RangeToDraw { get; set; }

        /// <summary>
        /// Gets or sets whether the parent tower can be placed.
        /// </summary>
        public bool TowerCanBePlaced
        {
            get
            {
                return _towerCanBePlaced;
            }
            set
            {
                _towerCanBePlaced = value;
                DrawRange();
            }
        }

        private bool _towerCanBePlaced = true;

        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            logger = new MethodLogger(nameof(Range));

            DrawRange();
        }

        /// <summary>
        /// Draws the range.
        /// </summary>
        public void DrawRange()
        {
            logger.Log($"Drawing range of {RangeToDraw}");

            // set sprite colour
            _spriteRenderer.color = TowerCanBePlaced ? CanBePlacedColour : CannotBePlacedColour;

            // width (thus height) of sprite in world space units
            var spriteSize = _spriteRenderer.sprite.bounds.size.x;

            // scalre required to bring sprite to size of the range
            var scale = RangeToDraw / spriteSize;

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

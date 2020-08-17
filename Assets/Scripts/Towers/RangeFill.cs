using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// Script for the range fill sprite of a tower.
    /// </summary>
    public class RangeFill : BaseBehaviour
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
                return towerCanBePlaced;
            }
            set
            {
                towerCanBePlaced = value;
                DrawRange();
            }
        }
        private bool towerCanBePlaced = true;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            logger = new MethodLogger(nameof(RangeFill));

            DrawRange();
        }

        /// <summary>
        /// Draws the range.
        /// </summary>
        public void DrawRange()
        {
            logger.Log($"Drawing range of {RangeToDraw}");

            // set sprite colour
            spriteRenderer.color = TowerCanBePlaced ? CanBePlacedColour : CannotBePlacedColour;

            // width (thus height) of sprite in world space units
            var spriteSize = spriteRenderer.sprite.bounds.size.x;

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

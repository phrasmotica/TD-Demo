using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Draws the health of an enemy.
    /// </summary>
    [ExecuteInEditMode]
    public class DrawHealth : MonoBehaviour
    {
        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer sprite;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer line;

        /// <summary>
        /// The enemy.
        /// </summary>
        private Enemy enemy;

        /// <summary>
        /// Whether to draw the health.
        /// </summary>
        private bool ShouldDrawHealth;

        /// <summary>
        /// The offset with which to render the health bar.
        /// </summary>
        [Range(0.1f, 0.5f)]
        public float OffsetY;

        /// <summary>
        /// Get reference to line renderer.
        /// </summary>
        private void Start()
        {
            sprite = gameObject.GetComponent<SpriteRenderer>();
            line = gameObject.GetComponent<LineRenderer>();
            enemy = gameObject.GetComponent<Enemy>();
        }

        /// <summary>
        /// Draw the range if necessary.
        /// </summary>
        private void Update()
        {
            line.forceRenderingOff = !ShouldDrawHealth;
            if (ShouldDrawHealth)
            {
                DrawHealthBar();
            }
        }

        /// <summary>
        /// Mouse is over the collider so draw the health.
        /// </summary>
        private void OnMouseEnter()
        {
            using (var logger = new MethodLogger(nameof(DrawRange)))
            {
                logger.Log("Drawing health");
                ShouldDrawHealth = true;
            }
        }

        /// <summary>
        /// Mouse is no longer over the collider so hide the health.
        /// </summary>
        private void OnMouseExit()
        {
            using (var logger = new MethodLogger(nameof(DrawRange)))
            {
                logger.Log("Not drawing health");
                ShouldDrawHealth = false;
            }
        }

        /// <summary>
        /// Draws the health bar.
        /// </summary>
        private void DrawHealthBar()
        {
            var spriteExtentX = sprite.sprite.bounds.extents.x;
            var spriteExtentY = sprite.sprite.bounds.extents.y;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * enemy.HealthFraction;

            line.SetPosition(0, new Vector3(start, spriteExtentY + OffsetY, 0));
            line.SetPosition(1, new Vector3(start + width, spriteExtentY + OffsetY, 0));
        }
    }
}

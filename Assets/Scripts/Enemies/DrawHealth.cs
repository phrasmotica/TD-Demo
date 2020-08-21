using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    /// <summary>
    /// Draws the health of an enemy.
    /// </summary>
    [ExecuteInEditMode]
    public class DrawHealth : BaseBehaviour
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
        /// Whether the mouse cursor is over the enemy.
        /// </summary>
        private bool mouseIsOverEnemy;

        /// <summary>
        /// The time in seconds to draw the enemy's health for when it's hit.
        /// </summary>
        [Range(0.5f, 3f)]
        public float PeekTime;

        /// <summary>
        /// The time in seconds left to draw the enemy's health for.
        /// </summary>
        private float remainingPeekTime;

        /// <summary>
        /// The offset with which to render the health bar.
        /// </summary>
        [Range(0.1f, 0.5f)]
        public float OffsetY;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            line = GetComponent<LineRenderer>();

            logger = new MethodLogger(nameof(DrawHealth));

            DrawHealthBar(GetComponent<Enemy>().HealthFraction);
        }

        /// <summary>
        /// Draw the health if necessary.
        /// </summary>
        private void Update()
        {
            if (remainingPeekTime > 0)
            {
                remainingPeekTime -= Time.deltaTime;
            }

            // draw health bar if we're in the peek period or if mouse is over the enemy
            line.forceRenderingOff = remainingPeekTime <= 0 && !mouseIsOverEnemy;
        }

        /// <summary>
        /// Mouse is over the collider so draw the health.
        /// </summary>
        private void OnMouseEnter()
        {
            logger.Log("Mouse is over the enemy");
            mouseIsOverEnemy = true;
        }

        /// <summary>
        /// Mouse is no longer over the collider so hide the health.
        /// </summary>
        private void OnMouseExit()
        {
            logger.Log("Mouse is no longer over the enemy");
            mouseIsOverEnemy = false;
        }

        /// <summary>
        /// Draws the health bar.
        /// </summary>
        private void DrawHealthBar(float healthFraction)
        {
            var spriteExtentX = sprite.sprite.bounds.extents.x;
            var spriteExtentY = sprite.sprite.bounds.extents.y;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * healthFraction;

            line.SetPosition(0, new Vector3(start, spriteExtentY + OffsetY, 0));
            line.SetPosition(1, new Vector3(start + width, spriteExtentY + OffsetY, 0));
        }

        /// <summary>
        /// Show the enemy's health briefly.
        /// </summary>
        public void PeekHealth()
        {
            remainingPeekTime = PeekTime;
        }

        /// <summary>
        /// Show the enemy's health briefly.
        /// </summary>
        public void PeekHealth(float newHealthFraction)
        {
            DrawHealthBar(newHealthFraction);
            PeekHealth();
        }
    }
}

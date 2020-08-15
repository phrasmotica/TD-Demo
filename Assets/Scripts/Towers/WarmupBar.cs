using UnityEngine;

namespace Assets.Scripts.Towers
{
    public class WarmupBar : MonoBehaviour
    {
        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer line;

        /// <summary>
        /// The parent tower's sprite renderer.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The offset with which to render the warmup bar.
        /// </summary>
        [Range(-0.5f, -0.1f)]
        public float OffsetY;

        /// <summary>
        /// Gets whether to draw the warmup bar.
        /// </summary>
        private bool ShouldDrawWarmupBar => GetComponentInParent<Tower>().State == TowerState.Warmup;

        /// <summary>
        /// Gets the warmup progress of the tower.
        /// </summary>
        private float WarmupProgress => GetComponentInParent<Tower>().WarmupProgress;

        /// <summary>
        /// Get the line renderer.
        /// </summary>
        private void Start()
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.useWorldSpace = false;

            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        /// <summary>
        /// Draw the warmup bar if necessary.
        /// </summary>
        private void Update()
        {
            line.forceRenderingOff = !ShouldDrawWarmupBar;
            if (ShouldDrawWarmupBar)
            {
                DrawWarmupBar();
            }
        }

        /// <summary>
        /// Draws the warmup bar.
        /// </summary>
        private void DrawWarmupBar()
        {
            var spriteExtentX = spriteRenderer.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * WarmupProgress;

            line.SetPosition(0, new Vector3(start, OffsetY, 0));
            line.SetPosition(1, new Vector3(start + width, OffsetY, 0));
        }
    }
}

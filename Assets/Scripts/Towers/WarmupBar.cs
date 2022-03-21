using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class WarmupBar : MonoBehaviour
    {
        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer _line;

        /// <summary>
        /// The parent tower's sprite renderer.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

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
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        /// <summary>
        /// Draw the warmup bar if necessary.
        /// </summary>
        private void Update()
        {
            _line.forceRenderingOff = !ShouldDrawWarmupBar;
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
            var spriteExtentX = _spriteRenderer.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * WarmupProgress;

            _line.SetPosition(0, new Vector3(start, OffsetY, 0));
            _line.SetPosition(1, new Vector3(start + width, OffsetY, 0));
        }
    }
}

using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class UpgradeBar : MonoBehaviour
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
        /// The offset with which to render the upgrade bar.
        /// </summary>
        [Range(-0.5f, -0.1f)]
        public float OffsetY;

        /// <summary>
        /// Gets whether to draw the upgrade bar.
        /// </summary>
        private bool ShouldDrawUpgradeBar => GetComponentInParent<TowerBehaviour>().IsUpgrading();

        /// <summary>
        /// Gets the upgrade progress of the tower.
        /// </summary>
        private float UpgradeProgress => GetComponentInParent<TowerBehaviour>().UpgradeProgress;

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
        /// Draw the upgrade bar if necessary.
        /// </summary>
        private void Update()
        {
            _line.forceRenderingOff = !ShouldDrawUpgradeBar;
            if (ShouldDrawUpgradeBar)
            {
                DrawUpgradeBar();
            }
        }

        /// <summary>
        /// Draws the upgrade bar.
        /// </summary>
        private void DrawUpgradeBar()
        {
            var spriteExtentX = _spriteRenderer.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * UpgradeProgress;

            _line.SetPosition(0, new Vector3(start, OffsetY, 0));
            _line.SetPosition(1, new Vector3(start + width, OffsetY, 0));
        }
    }
}

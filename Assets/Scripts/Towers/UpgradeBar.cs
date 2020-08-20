using UnityEngine;

namespace Assets.Scripts.Towers
{
    public class UpgradeBar : MonoBehaviour
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
        /// The offset with which to render the upgrade bar.
        /// </summary>
        [Range(-0.5f, -0.1f)]
        public float OffsetY;

        /// <summary>
        /// Gets whether to draw the upgrade bar.
        /// </summary>
        private bool ShouldDrawUpgradeBar => GetComponentInParent<Tower>().State == TowerState.Upgrading;

        /// <summary>
        /// Gets the upgrade progress of the tower.
        /// </summary>
        private float UpgradeProgress => GetComponentInParent<Tower>().UpgradeProgress;

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
        /// Draw the upgrade bar if necessary.
        /// </summary>
        private void Update()
        {
            line.forceRenderingOff = !ShouldDrawUpgradeBar;
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
            var spriteExtentX = spriteRenderer.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * UpgradeProgress;

            line.SetPosition(0, new Vector3(start, OffsetY, 0));
            line.SetPosition(1, new Vector3(start + width, OffsetY, 0));
        }
    }
}

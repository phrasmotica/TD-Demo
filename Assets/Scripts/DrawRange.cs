using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Draws the range of a tower.
    /// </summary>
    [ExecuteInEditMode]
    public class DrawRange : MonoBehaviour
    {
        /// <summary>
        /// The number of segments to draw.
        /// </summary>
        [Range(10, 100)]
        public int Segments;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer line;

        /// <summary>
        /// Whether to draw the radius.
        /// </summary>
        private bool ShouldDrawRadius;

        /// <summary>
        /// Get reference to line renderer.
        /// </summary>
        private void Start()
        {
            line = gameObject.GetComponent<LineRenderer>();
            line.positionCount = Segments + 1;
            line.useWorldSpace = false;
        }

        /// <summary>
        /// Draw the range if necessary.
        /// </summary>
        private void Update()
        {
            line.forceRenderingOff = !ShouldDrawRadius;
            if (ShouldDrawRadius)
            {
                DrawRadius();
            }
        }

        /// <summary>
        /// Mouse is over the collider so draw the radius.
        /// </summary>
        private void OnMouseEnter()
        {
            using (var logger = new MethodLogger(nameof(DrawRange)))
            {
                logger.Log("Drawing radius");
                ShouldDrawRadius = true;
            }
        }

        /// <summary>
        /// Mouse is no longer over the collider so hide the radius.
        /// </summary>
        private void OnMouseExit()
        {
            using (var logger = new MethodLogger(nameof(DrawRange)))
            {
                logger.Log("Not drawing radius");
                ShouldDrawRadius = false;
            }
        }

        /// <summary>
        /// Draws the radius.
        /// </summary>
        private void DrawRadius()
        {
            var shootProjectileComponent = gameObject.GetComponent<ShootProjectile>();
            var range = shootProjectileComponent.Range;

            line.positionCount = Segments + 1;

            float x;
            float y;

            float angle = 0;

            for (int i = 0; i < Segments + 1; i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

                line.SetPosition(i, new Vector3(x, y, 0));

                angle += 360f / Segments;
            }
        }
    }
}

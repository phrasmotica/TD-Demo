using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// Script for the range of a tower.
    /// </summary>
    public class Range : MonoBehaviour
    {
        /// <summary>
        /// The number of segments to draw.
        /// </summary>
        [Range(10, 100)]
        public int Segments;

        /// <summary>
        /// Gets or sets the range to draw.
        /// </summary>
        public int RangeToDraw
        {
            get
            {
                return rangeToDraw;
            }
            set
            {
                rangeToDraw = value;
                DrawRange();
            }
        }
        private int rangeToDraw;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer lineRenderer;

        /// <summary>
        /// Get reference to line renderer.
        /// </summary>
        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = Segments + 1;
            lineRenderer.useWorldSpace = false;

            DrawRange();
        }

        /// <summary>
        /// Draws the range.
        /// </summary>
        private void DrawRange()
        {
            using (var logger = new MethodLogger(nameof(Range)))
            {
                logger.Log($"Drawing range of {RangeToDraw}");
            }

            lineRenderer.positionCount = Segments + 1;

            float x;
            float y;

            float angle = 0;

            for (int i = 0; i < Segments + 1; i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * RangeToDraw;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * RangeToDraw;

                lineRenderer.SetPosition(i, new Vector3(x, y, 0));

                angle += 360f / Segments;
            }
        }
    }
}

using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// Script for the range of a tower.
    /// </summary>
    public class Range : BaseBehaviour
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
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            logger = new MethodLogger(nameof(Range));
        }

        /// <summary>
        /// Draws the range.
        /// </summary>
        private void DrawRange()
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
                lineRenderer.positionCount = Segments + 1;
                lineRenderer.useWorldSpace = false;
            }

            logger.Log($"Drawing range of {RangeToDraw}");

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

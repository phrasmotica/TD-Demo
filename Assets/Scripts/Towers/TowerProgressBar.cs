using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    /// <summary>
    /// Abstract script class for drawing a progress bar along the width of a tower.
    /// </summary>
    public abstract class TowerProgressBar : MonoBehaviour
    {
        public TowerBehaviour Tower;

        private LineRenderer _line;

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;
        }

        private void Update()
        {
            // TODO: only do this when progress starts or finishes
            // so we can remove the Tower field in this class
            _line.forceRenderingOff = !ShouldDraw();
        }

        public void DrawProgressBar(TowerBehaviour tower, float progress)
        {
            var sprite = tower.GetComponent<SpriteRenderer>();
            var spriteExtentX = sprite.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * progress;

            _line.SetPosition(0, new Vector3(start, 0, 0));
            _line.SetPosition(1, new Vector3(start + width, 0, 0));
        }

        protected abstract bool ShouldDraw();
    }
}

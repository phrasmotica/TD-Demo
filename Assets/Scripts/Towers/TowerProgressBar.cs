using UnityEngine;
using UnityEngine.Events;

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

            SetEventHandler(DrawUpgradeBar);
        }

        private void Update()
        {
            _line.forceRenderingOff = !ShouldDraw();
        }

        private void DrawUpgradeBar(float progress)
        {
            var sprite = Tower.GetComponent<SpriteRenderer>();
            var spriteExtentX = sprite.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * progress;

            _line.SetPosition(0, new Vector3(start, 0, 0));
            _line.SetPosition(1, new Vector3(start + width, 0, 0));
        }

        protected abstract void SetEventHandler(UnityAction<float> handler);

        protected abstract bool ShouldDraw();
    }
}

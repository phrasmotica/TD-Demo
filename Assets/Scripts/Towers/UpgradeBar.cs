using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class UpgradeBar : MonoBehaviour
    {
        public TowerBehaviour Tower;

        private LineRenderer _line;

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            Tower.OnUpgradeProgress += DrawUpgradeBar;
        }

        private void Update()
        {
            _line.forceRenderingOff = !ShouldDrawUpgradeBar();
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

        private bool ShouldDrawUpgradeBar() => Tower.IsUpgrading();
    }
}

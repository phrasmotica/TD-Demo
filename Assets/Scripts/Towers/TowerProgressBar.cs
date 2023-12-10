using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    /// <summary>
    /// Script for drawing a progress bar along the width of a tower.
    /// </summary>
    public class TowerProgressBar : MonoBehaviour
    {
        public SpriteRenderer TowerSprite;

        public LineRenderer Line;

        private void Start()
        {
            Line.enabled = false;
        }

        public void DrawProgressBar(TowerBehaviour tower, float progress)
        {
            var spriteExtentX = TowerSprite.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * progress;

            Line.SetPosition(0, new Vector3(start, 0, 0));
            Line.SetPosition(1, new Vector3(start + width, 0, 0));
        }
    }
}

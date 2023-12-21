using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Explosion : BaseBehaviour
    {
        public Transform ParentTransform;

        public SpriteRenderer SpriteRenderer;

        private void Awake()
        {
            logger = new MethodLogger(nameof(Explosion));
        }

        public void SetRadius(float radius)
        {
            logger.Log($"Setting radius to {radius}");

            // radius of explosion sprite in world space units
            var spriteSize = SpriteRenderer.sprite.bounds.size.x / 2;

            // scale required to bring sprite to size of the explosion
            var scale = radius / spriteSize;

            ParentTransform.localScale = new Vector3(scale, scale, 1);
        }
    }
}

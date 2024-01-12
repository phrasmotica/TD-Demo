using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerSelection : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public void SetSelected(bool isSelected)
        {
            SpriteRenderer.enabled = isSelected;
        }
    }
}

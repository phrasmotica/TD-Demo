using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerImage : MonoBehaviour
    {
        public Image Image;

        public void UpdateImage(TowerBehaviour tower)
        {
            if (tower != null)
            {
                Image.sprite = tower.SpriteRenderer.sprite;
            }
            else
            {
                Image.sprite = null;
            }
        }
    }
}

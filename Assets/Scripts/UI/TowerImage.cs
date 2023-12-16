using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerImage : MonoBehaviour
    {
        public void UpdateImage(TowerBehaviour tower)
        {
            if (tower != null)
            {
                GetComponent<Image>().sprite = tower.SpriteRenderer.sprite;
            }
            else
            { 
                GetComponent<Image>().sprite = null;
            }
        }
    }
}

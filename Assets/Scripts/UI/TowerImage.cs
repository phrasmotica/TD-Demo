using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerImage : MonoBehaviour
    {
        public void UpdateImage(TowerBehaviour tower)
        {
            GetComponent<Image>().sprite = tower.GetComponent<SpriteRenderer>().sprite;
        }
    }
}

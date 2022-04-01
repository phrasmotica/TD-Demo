using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerImage : MonoBehaviour
    {
        public TowerTooltip TowerTooltip;

        private void Awake()
        {
            TowerTooltip.OnSetTower += tower =>
            {
                GetComponent<Image>().sprite = tower.GetComponent<SpriteRenderer>().sprite;
            };
        }
    }
}

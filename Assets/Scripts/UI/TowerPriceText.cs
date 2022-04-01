using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerPriceText : MonoBehaviour
    {
        public TowerTooltip TowerTooltip;

        private void Awake()
        {
            TowerTooltip.OnSetTower += tower =>
            {
                GetComponent<Text>().text = $"Price: {tower.GetPrice()}";
            };
        }
    }
}

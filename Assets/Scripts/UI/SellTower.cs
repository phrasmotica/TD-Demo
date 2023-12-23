using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public Button Button;

        public Image Image;

        public void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;

            Button.interactable = canSellTower;
            Image.color = canSellTower ? ColourHelper.FullOpacity : ColourHelper.HalfOpacity;
        }
    }
}

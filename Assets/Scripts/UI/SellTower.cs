using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public SpriteRenderer Sprite;

        public Button Button;

        public void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;

            Button.interactable = canSellTower;
            Sprite.color = canSellTower ? ColourHelper.FullOpacity : ColourHelper.HalfOpacity;
        }
    }
}

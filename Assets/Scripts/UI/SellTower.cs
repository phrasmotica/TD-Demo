using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public SpriteRenderer Sprite;

        public void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;

            GetComponent<Button>().interactable = canSellTower;
            Sprite.color = canSellTower ? Color.white : Color.grey;
        }
    }
}

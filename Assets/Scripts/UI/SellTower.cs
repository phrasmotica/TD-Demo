using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public void SetState(bool canSellTower, int? sellPrice)
        {
            GetComponent<Button>().interactable = canSellTower;

            var text = canSellTower && sellPrice.HasValue ? $"Sell ({sellPrice.Value})" : "Sell";
            GetComponentInChildren<Text>().text = text;
        }
    }
}

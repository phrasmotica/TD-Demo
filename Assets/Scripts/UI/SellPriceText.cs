using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellPriceText : MonoBehaviour
    {
        public BankManager Bank;

        public Text Text;

        public void UpdateText(TowerBehaviour tower)
        {
            if (tower != null)
            {
                // TODO: achieve this without the reference to BankManager
                Text.text = $"{Bank.GetSellPrice(tower)}";
            }
            else
            {
                Text.text = string.Empty;
            }
        }
    }
}

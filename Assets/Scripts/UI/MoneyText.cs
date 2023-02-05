using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MoneyText : MonoBehaviour
    {
        public BankManager Bank;

        private void Awake()
        {
            Bank.OnMoneyChange += money =>
            {
                var text = GetComponent<Text>();
                text.text = $"Money: {money}";
            };
        }
    }
}

using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MoneyText : MonoBehaviour
    {
        public MoneyController MoneyController;

        private void Start()
        {
            MoneyController.OnMoneyChange += money =>
            {
                var text = GetComponent<Text>();
                text.text = $"Money: {money}";
            };
        }
    }
}
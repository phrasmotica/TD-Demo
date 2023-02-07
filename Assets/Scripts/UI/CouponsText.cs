using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class CouponsText : MonoBehaviour
    {
        public BankManager Bank;

        private void Awake()
        {
            Bank.OnCouponsChange += coupons =>
            {
                var text = GetComponent<Text>();
                text.text = $"{coupons}";
            };
        }
    }
}

using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    // TODO: change send wave, fast-forward and mute UI components to
    // buttons with icons, so there is more space for this, so it can
    // display larger values
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

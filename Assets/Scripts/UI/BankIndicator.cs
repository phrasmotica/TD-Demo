using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class BankIndicator : MonoBehaviour
    {
        public Text MoneyText;

        public Text CouponsText;

        public GameObject MoneyHighlight;

        public GameObject CouponsHighlight;

        public void UpdateMoney(int money)
        {
            MoneyText.text = $"{money}";
        }

        public void UpdateCoupons(int coupons)
        {
            CouponsText.text = $"{coupons}";
        }

        public void HighlightCurrency(bool useCoupons)
        {
            MoneyHighlight.SetActive(!useCoupons);

            CouponsHighlight.SetActive(useCoupons);
        }
    }
}

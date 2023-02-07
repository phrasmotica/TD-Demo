using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class PreferCouponsToggle : MonoBehaviour
    {
        public BankManager Bank;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(Toggle);
        }

        private void Toggle(bool useCoupons) => Bank.SetUseCoupons(useCoupons);
    }
}

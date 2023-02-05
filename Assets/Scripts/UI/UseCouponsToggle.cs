using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UseCouponsToggle : MonoBehaviour
    {
        public BankManager Bank;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(Toggle);

            Toggle(true);
        }

        private void Toggle(bool useCoupons) => Bank.SetUseCoupons(useCoupons);
    }
}

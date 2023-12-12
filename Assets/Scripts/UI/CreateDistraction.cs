using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Distractions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class CreateDistraction : MonoBehaviour
    {
        public DistractionSource DistractionSource;

        public BankManager Bank;

        public Button Button;

        public Image Image;

        public void SetInteractable()
        {
            var canAfford = Bank.CanAfford(DistractionSource.Price) != PurchaseMethod.None;
            Button.interactable = canAfford;
            Image.color = canAfford ? ColourHelper.FullOpacity : ColourHelper.HalfOpacity;
        }
    }
}

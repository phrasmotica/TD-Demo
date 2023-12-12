using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class CreateTower : MonoBehaviour
    {
        public GameObject TowerPrefab;

        public BankManager Bank;

        public Button Button;

        public Image Image;

        public void SetInteractable()
        {
            var tower = TowerPrefab.GetComponent<TowerBehaviour>();
            var canAfford = Bank.CanAffordToBuy(tower) != PurchaseMethod.None;
            Button.interactable = canAfford;
            Image.color = canAfford ? ColourHelper.FullOpacity : ColourHelper.HalfOpacity;
        }
    }
}

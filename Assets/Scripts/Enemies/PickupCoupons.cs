using TDDemo.Assets.Scripts.Controller;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class PickupCoupons : MonoBehaviour, IPickupItem
    {
        [Range(1, 2)]
        public int Amount;

        public void Pickup(PickupRouter pickupRouter)
        {
            pickupRouter.AddCouponsToBank(Amount);
        }
    }
}

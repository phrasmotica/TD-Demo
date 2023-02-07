using TDDemo.Assets.Scripts.Enemies;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    /// <summary>
    /// Class for proxying pickup item actions to various manager scripts.
    /// </summary>
    public class PickupRouter : MonoBehaviour
    {
        public BankManager Bank;

        public void AddCouponsToBank(IPickupItem source, int amount)
        {
            Bank.AddCoupons(amount);
            Bank.CreateCouponText((source as MonoBehaviour).transform.position, amount);
        }
    }
}

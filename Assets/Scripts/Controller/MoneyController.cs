using System;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class MoneyController : MonoBehaviour
    {
        [Range(10, 100)]
        public int StartingMoney;

        /// <summary>
        /// The current money.
        /// </summary>
        public int Money { get; private set; }

        /// <summary>
        /// Delegate to fire when the current amount of money changes.
        /// </summary>
        public event Action<int> OnMoneyChange;

        public void Start()
        {
            ResetMoney();
        }

        public bool CanAfford(int price) => price <= Money;

        public void AddMoney(int amount) => SetMoney(Money + amount);

        public void SetMoney(int money)
        {
            Money = money;
            OnMoneyChange(money);
        }

        public void ResetMoney() => SetMoney(StartingMoney);
    }
}

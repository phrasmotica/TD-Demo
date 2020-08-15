using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class MoneyController : MonoBehaviour
    {
        /// <summary>
        /// The starting money.
        /// </summary>
        [Range(10, 100)]
        public int StartingMoney;

        /// <summary>
        /// The create tower script.
        /// </summary>
        public CreateTower CreateTower;

        /// <summary>
        /// The current money.
        /// </summary>
        public int Money
        {
            get
            {
                return money;
            }
            private set
            {
                money = value;
                MoneyText.text = $"Money: {money}";
                CreateTower.SetInteractable(money);
            }
        }
        private int money;

        /// <summary>
        /// The text used to display the money.
        /// </summary>
        public Text MoneyText;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        public void Start()
        {
            ResetMoney();
        }

        /// <summary>
        /// Returns whether we can afford the given price.
        /// </summary>
        public bool CanAfford(int price)
        {
            return price <= Money;
        }

        /// <summary>
        /// Adds the given amount of money to the current pot.
        /// </summary>
        public void AddMoney(int amount)
        {
            Money += amount;
        }

        /// <summary>
        /// Resets to the starting money.
        /// </summary>
        public void ResetMoney()
        {
            Money = StartingMoney;
        }
    }
}

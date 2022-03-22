using TDDemo.Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Controller
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
        public int Money { get; private set; }

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
        public bool CanAfford(int price) => price <= Money;

        /// <summary>
        /// Adds the given amount of money to the current pot.
        /// </summary>
        public void AddMoney(int amount) => SetMoney(Money + amount);

        public void SetMoney(int money)
        {
            Money = money;
            MoneyText.text = $"Money: {money}";
            CreateTower.SetInteractable(price => CanAfford(price));
        }

        /// <summary>
        /// Resets to the starting money.
        /// </summary>
        public void ResetMoney() => SetMoney(StartingMoney);
    }
}

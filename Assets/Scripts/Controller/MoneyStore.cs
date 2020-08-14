using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class MoneyStore : MonoBehaviour
    {
        /// <summary>
        /// The starting money.
        /// </summary>
        [Range(10, 100)]
        public int StartingMoney;

        /// <summary>
        /// The current money.
        /// </summary>
        private int Money;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        public void Start()
        {
            Money = StartingMoney;
        }

        /// <summary>
        /// Adds the given amount of money to the current pot.
        /// </summary>
        public void AddMoney(int amount)
        {
            Money += amount;
        }

        /// <summary>
        /// Renders a GUI.
        /// </summary>
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 30, 160, 30), $"Money {Money}");
        }
    }
}

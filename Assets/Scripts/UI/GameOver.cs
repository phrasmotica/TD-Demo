using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        public BankManager Bank;

        public GameObject GameOverScreen;

        public Text FinalMoneyText;

        public void ShowGameOverScreen()
        {
            GameOverScreen.SetActive(true);

            FinalMoneyText.text = $"You finished with {Bank.Money} money.";
        }
    }
}

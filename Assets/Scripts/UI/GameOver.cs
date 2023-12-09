using TDDemo.Assets.Scripts.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDDemo.Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        public BankManager Bank;

        public TMP_Text FinalMoneyText;

        public void UpdateText()
        {
            FinalMoneyText.text = $"You finished with {Bank.Money} money.";
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}

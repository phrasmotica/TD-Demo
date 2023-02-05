using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        public LivesController LivesController;

        public BankManager Bank;

        public GameObject GameOverScreen;

        public Text FinalMoneyText;

        public Button RestartButton;

        public event UnityAction OnRestart;

        private void Awake()
        {
            LivesController.OnEndGame += () =>
            {
                GameOverScreen.SetActive(true);

                FinalMoneyText.text = $"You finished with {Bank.Money} money.";

                RestartButton.onClick.AddListener(Restart);
            };
        }

        private void Restart()
        {
            GameOverScreen.SetActive(false);
            OnRestart?.Invoke();
        }
    }
}

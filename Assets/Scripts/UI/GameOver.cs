using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Music;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        public TowerController TowerController;

        public TowerManager TowerManager;

        public LivesController LivesController;

        public MoneyController MoneyController;

        public MusicController MusicController;

        public WavesController WavesController;

        public GameObject GameOverScreen;

        public Text FinalMoneyText;

        public Button RestartButton;

        private void Awake()
        {
            LivesController.OnEndGame += EndGame;
        }

        public void EndGame()
        {
            TowerController.CancelCreateTower();

            GameOverScreen.SetActive(true);

            FinalMoneyText.text = $"You finished with {MoneyController.Money} money.";

            RestartButton.onClick.AddListener(Restart);
        }

        public void Restart()
        {
            WavesController.StopAllCoroutines();
            WavesController.ResetWaves();
            WavesController.ClearEnemies();

            MoneyController.ResetMoney();

            LivesController.ResetLives();

            MusicController.StartMusic();

            TowerManager.DeselectCurrentTower();
            TowerManager.ClearTowers();

            GameOverScreen.SetActive(false);
        }
    }
}

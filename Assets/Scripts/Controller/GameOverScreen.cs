using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class GameOverScreen : MonoBehaviour
    {
        /// <summary>
        /// The game over canvas prefab.
        /// </summary>
        public GameObject GameOverCanvasPrefab;

        /// <summary>
        /// The instantiated game over canvas.
        /// </summary>
        private GameObject gameOverCanvas;

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            gameOverCanvas = Instantiate(GameOverCanvasPrefab);

            var moneyStore = GetComponent<MoneyStore>();
            var finishingMoney = moneyStore.Money;

            var panelTransform = gameOverCanvas.transform.Find("Panel");
            var moneyText = panelTransform.Find("MoneyText");
            moneyText.GetComponent<Text>().text = $"You finished with {finishingMoney} money.";

            var restartButton = panelTransform.Find("RestartButton");
            restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        }

        /// <summary>
        /// Resets components ready for a new game.
        /// </summary>
        private void RestartGame()
        {
            var waveController = GetComponent<CreateWaves>();
            waveController.ResetWaves();

            var moneyController = GetComponent<MoneyStore>();
            moneyController.ResetMoney();

            var livesController = GetComponent<LivesCounter>();
            livesController.ResetLives();

            Destroy(gameOverCanvas);
        }
    }
}

using Assets.Scripts.Util;
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

            var moneyController = GetComponent<MoneyController>();
            var finishingMoney = moneyController.Money;

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
            var wavesController = GetComponent<WavesController>();
            wavesController.ResetWaves();

            var moneyController = GetComponent<MoneyController>();
            moneyController.ResetMoney();

            var livesController = GetComponent<LivesController>();
            livesController.ResetLives();

            var towers = GameObject.FindGameObjectsWithTag(Tags.TowerTag);
            foreach (var t in towers)
            {
                Destroy(t);
            }

            Destroy(gameOverCanvas);
        }
    }
}

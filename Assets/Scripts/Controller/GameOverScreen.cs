using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Controller
{
    public class GameOverScreen : MonoBehaviour
    {
        public TowerController TowerController;

        public TowerManager TowerManager;

        public LivesController LivesController;

        public MoneyController MoneyController;

        public WavesController WavesController;

        /// <summary>
        /// The game over canvas prefab.
        /// </summary>
        public GameObject GameOverCanvasPrefab;

        /// <summary>
        /// The instantiated game over canvas.
        /// </summary>
        private GameObject _gameOverCanvas;

        // TODO: add OnGameOver event

        private void Start()
        {
            LivesController.OnEndGame += EndGame;
        }

        public void EndGame()
        {
            _gameOverCanvas = Instantiate(GameOverCanvasPrefab);

            var finishingMoney = MoneyController.Money;

            var panelTransform = _gameOverCanvas.transform.Find("Panel");
            var moneyText = panelTransform.Find("MoneyText");
            moneyText.GetComponent<Text>().text = $"You finished with {finishingMoney} money.";

            var restartButton = panelTransform.Find("RestartButton");
            restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);

            TowerController.CancelCreateTower();
        }

        /// <summary>
        /// Resets components ready for a new game.
        /// </summary>
        private void RestartGame()
        {
            WavesController.StopAllCoroutines();
            WavesController.ResetWaves();

            MoneyController.ResetMoney();

            LivesController.ResetLives();

            TowerManager.DeselectCurrentTower();

            var towers = GameObject.FindGameObjectsWithTag(Tags.Tower);
            foreach (var t in towers)
            {
                Destroy(t);
            }

            var enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy);
            foreach (var e in enemies)
            {
                Destroy(e);
            }

            Destroy(_gameOverCanvas);
        }
    }
}

using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Controller
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
        private GameObject _gameOverCanvas;

        private TowerController _towerController;

        private TowerManager _towerManager;

        private void Start()
        {
            _towerController = GetComponent<TowerController>();
            _towerManager = GetComponent<TowerManager>();
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            _gameOverCanvas = Instantiate(GameOverCanvasPrefab);

            var moneyController = GetComponent<MoneyController>();
            var finishingMoney = moneyController.Money;

            var panelTransform = _gameOverCanvas.transform.Find("Panel");
            var moneyText = panelTransform.Find("MoneyText");
            moneyText.GetComponent<Text>().text = $"You finished with {finishingMoney} money.";

            var restartButton = panelTransform.Find("RestartButton");
            restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);

            _towerController.CancelCreateTower();
        }

        /// <summary>
        /// Resets components ready for a new game.
        /// </summary>
        private void RestartGame()
        {
            var wavesController = GetComponent<WavesController>();
            wavesController.StopAllCoroutines();
            wavesController.ResetWaves();

            var moneyController = GetComponent<MoneyController>();
            moneyController.ResetMoney();

            var livesController = GetComponent<LivesController>();
            livesController.ResetLives();

            _towerManager.DeselectCurrentTower();

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

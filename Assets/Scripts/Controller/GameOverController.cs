using TDDemo.Assets.Scripts.UI;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class GameOverController : MonoBehaviour
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

        private void Start()
        {
            LivesController.OnEndGame += EndGame;
        }

        public void EndGame()
        {
            _gameOverCanvas = Instantiate(GameOverCanvasPrefab);

            // TODO: decouple this UI script from this controller somehow...
            var gameOver = _gameOverCanvas.GetComponent<GameOver>();
            gameOver.TowerController = TowerController;
            gameOver.TowerManager = TowerManager;
            gameOver.LivesController = LivesController;
            gameOver.MoneyController = MoneyController;
            gameOver.WavesController = WavesController;
            gameOver.GameOverController = this;

            TowerController.CancelCreateTower();
        }

        public void RestartGame()
        {
            Destroy(_gameOverCanvas);
        }
    }
}

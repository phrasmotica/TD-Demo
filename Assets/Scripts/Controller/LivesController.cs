using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Controller
{
    public class LivesController : MonoBehaviour
    {
        /// <summary>
        /// The number of starting lives.
        /// </summary>
        [Range(1, 50)]
        public int StartingLives;

        /// <summary>
        /// The game over screen script.
        /// </summary>
        private GameOverScreen _gameOverScreen;

        /// <summary>
        /// The current number of lives.
        /// </summary>
        private int _lives;

        /// <summary>
        /// Gets whether the game is over.
        /// </summary>
        private bool GameIsOver => _lives <= 0;

        /// <summary>
        /// The text used to display the lives.
        /// </summary>
        public Text LivesText;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _gameOverScreen = gameObject.GetComponent<GameOverScreen>();
            ResetLives();
        }

        /// <summary>
        /// Adds the given amount of lives to the current count.
        /// </summary>
        public void AddLives(int amount)
        {
            if (!GameIsOver)
            {
                SetLives(_lives + amount);
                CheckForGameEnd();
            }
        }

        public void SetLives(int lives)
        {
            _lives = lives;
            LivesText.text = $"Lives: {lives}";
        }

        /// <summary>
        /// Resets to the starting lives.
        /// </summary>
        public void ResetLives() => SetLives(StartingLives);

        /// <summary>
        /// Checks if the game should end, and if so ends the game.
        /// </summary>
        private void CheckForGameEnd()
        {
            if (GameIsOver)
            {
                _gameOverScreen.EndGame();
            }
        }
    }
}

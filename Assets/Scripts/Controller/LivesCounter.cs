﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class LivesCounter : MonoBehaviour
    {
        /// <summary>
        /// The number of starting lives.
        /// </summary>
        [Range(1, 50)]
        public int StartingLives;

        /// <summary>
        /// The game over screen script.
        /// </summary>
        private GameOverScreen gameOverScreen;

        /// <summary>
        /// The current number of lives.
        /// </summary>
        private int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                LivesText.text = $"Lives: {lives}";
            }
        }
        private int lives;

        /// <summary>
        /// The text used to display the lives.
        /// </summary>
        public Text LivesText;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            gameOverScreen = gameObject.GetComponent<GameOverScreen>();
            Lives = StartingLives;
        }

        /// <summary>
        /// Adds the given amount of lives to the current count.
        /// </summary>
        public void AddLives(int amount)
        {
            Lives += amount;
            CheckForGameEnd();
        }

        /// <summary>
        /// Checks if the game should end, and if so ends the game.
        /// </summary>
        private void CheckForGameEnd()
        {
            if (Lives <= 0)
            {
                gameOverScreen.EndGame();
            }
        }
    }
}

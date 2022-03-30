using System;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class LivesController : MonoBehaviour
    {
        [Range(1, 50)]
        public int StartingLives;

        /// <summary>
        /// The current number of lives.
        /// </summary>
        private int _lives;

        /// <summary>
        /// Delegate to fire when the current amount of lives changes.
        /// </summary>
        public event Action<int> OnLivesChange;

        /// <summary>
        /// Delegate to fire when the number of lives reaches zero.
        /// </summary>
        public event Action OnEndGame;

        private void Start()
        {
            ResetLives();
        }

        public void AddLives(int amount)
        {
            if (!GameIsOver())
            {
                SetLives(_lives + amount);

                if (GameIsOver())
                {
                    OnEndGame?.Invoke();
                }
            }
        }

        private void SetLives(int lives)
        {
            _lives = lives;
            OnLivesChange?.Invoke(lives);
        }

        public void ResetLives() => SetLives(StartingLives);

        private bool GameIsOver() => _lives <= 0;
    }
}

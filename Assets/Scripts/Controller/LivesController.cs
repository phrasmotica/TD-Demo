using System;
using TDDemo.Assets.Scripts.Path;
using TDDemo.Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class LivesController : MonoBehaviour
    {
        public EndZone EndZone;

        public GameOver GameOver;

        [Range(1, 50)]
        public int StartingLives;

        /// <summary>
        /// The current number of lives.
        /// </summary>
        private int _lives;

        /// <summary>
        /// Delegate to fire when the current amount of lives changes.
        /// </summary>
        public event UnityAction<int> OnLivesChange;

        /// <summary>
        /// Delegate to fire when the number of lives reaches zero.
        /// </summary>
        public event UnityAction OnEndGame;

        private void Start()
        {
            OnLivesChange += lives =>
            {
                _lives = lives;

                if (lives <= 0)
                {
                    OnEndGame?.Invoke();
                }
            };

            EndZone.OnEnemyCollide += obj => AddLives(-1);

            GameOver.OnRestart += ResetLives;

            ResetLives();
        }

        public void AddLives(int amount) => SetLives(_lives + amount);

        public void ResetLives() => SetLives(StartingLives);

        private void SetLives(int lives) => OnLivesChange(lives);
    }
}

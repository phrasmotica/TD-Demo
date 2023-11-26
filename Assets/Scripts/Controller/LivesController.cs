using System;
using TDDemo.Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class LivesController : MonoBehaviour
    {
        [Range(1, 50)]
        public int StartingLives;

        private int _lives;

        public UnityEvent<int> OnLivesChange;

        public UnityEvent OnEndGame;

        private void Start()
        {
            ResetLives();
        }

        public void HandleEnemy(Enemy enemy)
        {
            AddLives(-enemy.Strength);
        }

        public void AddLives(int amount) => SetLives(Math.Max(0, _lives + amount));

        public void ResetLives() => SetLives(StartingLives);

        private void SetLives(int lives)
        {
            _lives = lives;

            OnLivesChange.Invoke(_lives);

            if (_lives <= 0)
            {
                OnEndGame.Invoke();
            }
        }
    }
}

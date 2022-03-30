using System;
using System.Collections;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class WavesController : BaseBehaviour
    {
        public MoneyController MoneyController;

        /// <summary>
        /// The enemy prefab.
        /// </summary>
        public GameObject EnemyPrefab;

        /// <summary>
        /// The current wave.
        /// </summary>
        private int _currentWave;

        /// <summary>
        /// Delegate to fire when the wave number changes.
        /// </summary>
        public event Action<int> OnWaveChange;

        public void Start()
        {
            logger = new MethodLogger(nameof(WavesController));

            ResetWaves();
            Physics2D.gravity = Vector2.zero;
        }

        private void SetCurrentWave(int currentWave)
        {
            _currentWave = currentWave;
            OnWaveChange?.Invoke(currentWave);
        }

        public void DoSendNextWave()
        {
            SetCurrentWave(_currentWave + 1);
            StartCoroutine(SendWave(_currentWave));
        }

        private IEnumerator SendWave(int waveNumber)
        {
            logger.Log($"SendWave({waveNumber})");

            var enemyCount = GetEnemyCount(waveNumber);

            for (var i = 0; i < enemyCount; i++)
            {
                var enemy = Instantiate(EnemyPrefab);
                enemy.GetComponent<Enemy>().OnKill += MoneyController.AddMoney;

                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        /// Returns the number of enemies to spawn for the given wave.
        /// </summary>
        private int GetEnemyCount(int waveNumber) => waveNumber;

        /// <summary>
        /// Resets to wave zero, ready for the start of a game.
        /// </summary>
        public void ResetWaves() => SetCurrentWave(0);

        public void ResetEnemies()
        {
            var enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy);
            foreach (var e in enemies)
            {
                Destroy(e);
            }
        }
    }
}

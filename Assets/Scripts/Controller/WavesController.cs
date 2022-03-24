using System.Collections;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Controller
{
    public class WavesController : BaseBehaviour
    {
        /// <summary>
        /// The current wave.
        /// </summary>
        private int _currentWave;

        /// <summary>
        /// The text used to display the waves.
        /// </summary>
        public Text WaveText;

        /// <summary>
        /// The text on the button used for sending the next wave.
        /// </summary>
        public Text SendWaveButtonText;

        /// <summary>
        /// The number of waves.
        /// </summary>
        [Range(1, 5)]
        public int NumberOfWaves;

        /// <summary>
        /// The enemy prefab.
        /// </summary>
        public GameObject EnemyPrefab;

        /// <summary>
        /// The money controller.
        /// </summary>
        private MoneyController _moneyController;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        public void Start()
        {
            _moneyController = GetComponent<MoneyController>();

            logger = new MethodLogger(nameof(WavesController));

            ResetWaves();
            Physics2D.gravity = Vector2.zero;
        }

        public void SetCurrentWave(int currentWave)
        {
            _currentWave = currentWave;
            WaveText.text = $"Wave {currentWave}";
            SendWaveButtonText.text = $"Send wave {currentWave + 1}";
        }

        /// <summary>
        /// Starts the coroutine to send the next wave.
        /// </summary>
        public void DoSendNextWave()
        {
            SetCurrentWave(_currentWave + 1);
            StartCoroutine(SendWave(_currentWave));
        }

        /// <summary>
        /// Sends the given wave.
        /// </summary>
        public IEnumerator SendWave(int waveNumber)
        {
            logger.Log($"SendWave({waveNumber})");

            var enemyCount = GetEnemyCount(waveNumber);

            for (var i = 0; i < enemyCount; i++)
            {
                var enemy = Instantiate(EnemyPrefab);
                enemy.GetComponent<Enemy>().OnKill = reward => _moneyController.AddMoney(reward);

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
    }
}

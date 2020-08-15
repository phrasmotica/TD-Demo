using System.Collections;
using Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class WavesController : MonoBehaviour
    {
        /// <summary>
        /// The current wave.
        /// </summary>
        private int CurrentWave
        {
            get
            {
                return currentWave;
            }
            set
            {
                currentWave = value;
                WaveText.text = $"Wave {currentWave}";
                SendWaveButtonText.text = $"Send wave {currentWave + 1}";
            }
        }
        private int currentWave;

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
        private MoneyController MoneyController;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        public void Start()
        {
            MoneyController = gameObject.GetComponent<MoneyController>();

            ResetWaves();
            Physics2D.gravity = Vector2.zero;
        }

        /// <summary>
        /// Starts the coroutine to send the next wave.
        /// </summary>
        public void DoSendNextWave()
        {
            StartCoroutine(SendWave(++CurrentWave));
        }

        /// <summary>
        /// Sends the given wave.
        /// </summary>
        public IEnumerator SendWave(int waveNumber)
        {
            Debug.Log($"SendWave({waveNumber})");

            var enemyCount = GetEnemyCount(waveNumber);

            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = Instantiate(EnemyPrefab);
                enemy.GetComponent<Enemy>().OnKill = (reward) => MoneyController.AddMoney(reward);

                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        /// Returns the number of enemies to spawn for the given wave.
        /// </summary>
        private int GetEnemyCount(int waveNumber)
        {
            return waveNumber;
        }

        /// <summary>
        /// Resets to wave zero, ready for the start of a game.
        /// </summary>
        public void ResetWaves()
        {
            CurrentWave = 0;
        }
    }
}

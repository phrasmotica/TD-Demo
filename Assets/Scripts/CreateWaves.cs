using System.Collections;
using Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CreateWaves : MonoBehaviour
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
            }
        }
        private int currentWave;

        /// <summary>
        /// The text used to display the waves.
        /// </summary>
        public Text WaveText;

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
        /// The money store.
        /// </summary>
        private MoneyStore MoneyStore;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        public void Start()
        {
            MoneyStore = gameObject.GetComponent<MoneyStore>();

            Physics2D.gravity = Vector2.zero;
        }

        /// <summary>
        /// Starts the waves.
        /// </summary>
        public void StartWaves()
        {
            Debug.Log("StartWaves()");

            StartCoroutine(SendAllWaves());
        }

        /// <summary>
        /// Sends all the waves on a timer.
        /// </summary>
        public IEnumerator SendAllWaves()
        {
            Debug.Log("SendAllWaves()");

            for (int i = 1; i <= NumberOfWaves; i++)
            {
                yield return StartCoroutine(SendWave(i));
                yield return new WaitForSeconds(3);
            }
        }

        /// <summary>
        /// Sends the given wave.
        /// </summary>
        public IEnumerator SendWave(int waveNumber)
        {
            Debug.Log($"SendWave({waveNumber})");

            CurrentWave = waveNumber;

            var enemyCount = GetEnemyCount(waveNumber);

            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = Instantiate(EnemyPrefab);
                enemy.GetComponent<Enemy>().OnKill = (reward) => MoneyStore.AddMoney(reward);

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
    }
}

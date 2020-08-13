using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CreateWaves : MonoBehaviour
    {
        /// <summary>
        /// The current wave.
        /// </summary>
        private int CurrentWave;

        /// <summary>
        /// The current money.
        /// </summary>
        private int Money;

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
        /// Start is called before the first frame update.
        /// </summary>
        public void Start()
        {
            Physics2D.gravity = Vector2.zero;
            Money = 0;
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
                var enemy = Instantiate(EnemyPrefab, new Vector3(-7, 0, 0), Quaternion.identity);
                enemy.GetComponent<Enemy>().OnKill = (reward) => Money += reward;

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
        /// Renders a GUI.
        /// </summary>
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 160, 30), $"Wave {CurrentWave}");
            GUI.Label(new Rect(0, 30, 160, 30), $"Money {Money}");
        }
    }
}

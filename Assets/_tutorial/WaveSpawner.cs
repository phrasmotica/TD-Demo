using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo
{
    public class WaveSpawner : MonoBehaviour
    {
        public Transform EnemyPrefab;

        public Transform SpawnPoint;

        public float TimeBetweenWaves = 5f;

        private int _waveNumber;

        private float _countdown = 2f;

        public UnityEvent<float> OnCountdown;

        private void Update()
        {
            if (_countdown <= 0f)
            {
                StartCoroutine(SpawnWave());

                _countdown = TimeBetweenWaves;
            }

            _countdown -= Time.deltaTime;

            OnCountdown.Invoke(_countdown);
        }

        private IEnumerator SpawnWave()
        {
            _waveNumber++;

            for (var i = 0; i < _waveNumber; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void SpawnEnemy()
        {
            Instantiate(EnemyPrefab, SpawnPoint.position, SpawnPoint.rotation);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Path;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class WavesController : BaseBehaviour
    {
        public MoneyController MoneyController;

        public GameObject EnemyPrefab;

        public Waypoint[] Waypoints;

        /// <summary>
        /// The current wave.
        /// </summary>
        private int _currentWave;

        private List<Enemy> _enemies;

        /// <summary>
        /// Delegate to fire when the wave number changes.
        /// </summary>
        public event Action<int> OnWaveChange;

        public void Start()
        {
            _enemies = new List<Enemy>();

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
                var firstWaypointPos = Waypoints.First().transform.position;
                var enemyObj = Instantiate(EnemyPrefab, firstWaypointPos, Quaternion.identity);

                var waypointFollower = enemyObj.GetComponent<WaypointFollower>();
                waypointFollower.Waypoints = Waypoints;

                var enemy = enemyObj.GetComponent<Enemy>();
                enemy.OnKill += e =>
                {
                    MoneyController.AddMoney(e.Reward);
                    RemoveEnemy(e);
                };

                _enemies.Add(enemy);

                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        /// Returns the number of enemies to spawn for the given wave.
        /// </summary>
        private int GetEnemyCount(int waveNumber) => waveNumber;

        public List<GameObject> GetEnemies() => _enemies.Select(e => e.gameObject).ToList();

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        /// <summary>
        /// Resets to wave zero, ready for the start of a game.
        /// </summary>
        public void ResetWaves() => SetCurrentWave(0);

        public void ClearEnemies()
        {
            foreach (var e in _enemies)
            {
                Destroy(e.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Path;
using TDDemo.Assets.Scripts.UI;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class WavesController : BaseBehaviour
    {
        public MoneyController MoneyController;

        public EndZone EndZone;

        public GameOver GameOver;

        public GameObject EnemyPrefab;

        public Waypoint[] Waypoints;

        private int _currentWave;

        private List<Enemy> _enemies;

        public event UnityAction<int> OnWaveChange;

        public event UnityAction<int> OnStageChange;

        public event UnityAction<List<GameObject>> OnEnemiesChange;

        public void Start()
        {
            _enemies = new List<Enemy>();
            OnEnemiesChange?.Invoke(GetEnemies());

            logger = new MethodLogger(nameof(WavesController));

            OnWaveChange += currentWave => _currentWave = currentWave;

            EndZone.OnEnemyCollide += RemoveEnemy;

            GameOver.OnRestart += () =>
            {
                StopAllCoroutines();
                ResetWaves();
                ClearEnemies();
            };

            ResetWaves();
            Physics2D.gravity = Vector2.zero;
        }

        private void SetCurrentWave(int currentWave)
        {
            OnWaveChange(currentWave);

            if (IsStartOfStage(currentWave))
            {
                var stageNumber = GetStageNumber(currentWave);
                OnStageChange?.Invoke(stageNumber);
            }
        }

        private static bool IsStartOfStage(int currentWave) => currentWave % 5 == 1;

        private static int GetStageNumber(int currentWave) => currentWave / 5 + 1;

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
                enemy.OnKill += (e, tower) =>
                {
                    MoneyController.AddReward(e, tower);
                    RemoveEnemy(e);
                };

                _enemies.Add(enemy);
                OnEnemiesChange?.Invoke(GetEnemies());

                yield return new WaitForSeconds(1);
            }
        }

        private int GetEnemyCount(int waveNumber) => waveNumber;

        public List<GameObject> GetEnemies() => _enemies.Select(e => e.gameObject).ToList();

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            OnEnemiesChange?.Invoke(GetEnemies());

            Destroy(enemy.gameObject);
        }

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

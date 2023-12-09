using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Path;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WavesController : BaseBehaviour
    {
        public BankManager Bank;

        public PickupRouter PickupRouter;

        public GameObject EnemyPrefab;

        public GameObject BossEnemyPrefab;

        public Waypoint[] Waypoints;

        public Wave[] Waves;

        private int _currentWave;

        private List<Enemy> _enemies;

        public UnityEvent<int> OnWaveChange;

        public UnityEvent<int> OnStageChange;

        public UnityEvent<List<GameObject>> OnEnemiesChange;

        public void Start()
        {
            _enemies = new List<Enemy>();
            OnEnemiesChange.Invoke(GetEnemies());

            logger = new MethodLogger(nameof(WavesController));

            ResetWaves();
            Physics2D.gravity = Vector2.zero;
        }

        private void SetCurrentWave(int currentWave)
        {
            _currentWave = currentWave;
            OnWaveChange.Invoke(_currentWave);

            if (IsStartOfStage(_currentWave))
            {
                var stageNumber = GetStageNumber(_currentWave);
                OnStageChange.Invoke(stageNumber);
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

            var wave = GetWave(waveNumber);

            logger.Log($"Spawning {wave.EnemyPrefab.name} x{wave.Count} at {wave.Frequency} per second");

            for (var i = 0; i < wave.Count; i++)
            {
                var firstWaypointPos = Waypoints.First().transform.position;
                var enemyObj = Instantiate(wave.EnemyPrefab, firstWaypointPos, Quaternion.identity);

                var waypointFollower = enemyObj.GetComponent<WaypointFollower>();
                waypointFollower.Waypoints = Waypoints;

                var enemy = enemyObj.GetComponent<Enemy>();

                var itemDrops = enemy.GetComponents<ItemDropper>();

                foreach (var item in itemDrops)
                {
                    item.PickupRouter = PickupRouter;
                }

                // cannot be set in the editor because the enemy is not known ahead of time
                enemy.OnKill.AddListener(HandleEnemyKill);

                _enemies.Add(enemy);
                OnEnemiesChange.Invoke(GetEnemies());

                yield return new WaitForSeconds(1f / wave.Frequency);
            }
        }

        private void HandleEnemyKill(Enemy e, TowerBehaviour tower)
        {
            Bank.AddReward(e, tower);
            RemoveEnemy(e);
        }

        private Wave GetWave(int waveNumber)
        {
            if (waveNumber - 1 < Waves.Length)
            {
                return Waves[waveNumber - 1];
            }

            if (waveNumber % 5 == 0)
            {
                return new Wave
                {
                    EnemyPrefab = BossEnemyPrefab,
                    Count = waveNumber / 5,
                    Frequency = 1,
                };
            }

            return new Wave
            {
                EnemyPrefab = EnemyPrefab,
                Count = waveNumber,
                Frequency = 2,
            };
        }

        public List<GameObject> GetEnemies() => _enemies.Select(e => e.gameObject).ToList();

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            OnEnemiesChange.Invoke(GetEnemies());
        }

        public void ResetAll()
        {
            StopAllCoroutines();
            ResetWaves();
            ClearEnemies();
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

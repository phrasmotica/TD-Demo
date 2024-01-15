using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Path;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Effects;
using TDDemo.Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WavesController : BaseBehaviour
    {
        public GameObject Canvas;

        public BankManager Bank;

        public PickupRouter PickupRouter;

        public GameObject EnemyPrefab;

        public GameObject BossEnemyPrefab;

        public GameObject DistractionTextPrefab;

        public Waypoint[] Waypoints;

        public Wave[] Waves;

        private int _currentWaveNumber;

        private List<Enemy> _enemies;

        public UnityEvent<List<Wave>> OnWavesChange;

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

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.N))
            {
                DoSendNextWave();
            }
        }

        private Wave SetCurrentWave(int waveNumber)
        {
            _currentWaveNumber = waveNumber;

            var nextWaves = GetNextWaves(_currentWaveNumber);

            OnWavesChange.Invoke(nextWaves);

            if (IsStartOfStage(_currentWaveNumber))
            {
                var stageNumber = GetStageNumber(_currentWaveNumber);
                OnStageChange.Invoke(stageNumber);
            }

            return nextWaves[0];
        }

        private static bool IsStartOfStage(int currentWave) => currentWave % 5 == 1;

        private static int GetStageNumber(int currentWave) => currentWave / 5 + 1;

        public void DoSendNextWave()
        {
            var wave = SetCurrentWave(_currentWaveNumber + 1);
            StartCoroutine(SendWave(wave));
        }

        private IEnumerator SendWave(Wave wave)
        {
            logger.Log($"SendWave({_currentWaveNumber})");

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
                enemy.OnEffect.AddListener(HandleEnemyEffect);
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
                var wave = Waves[Mathf.Max(0, waveNumber - 1)];

                wave.Number = waveNumber;

                return wave;
            }

            if (waveNumber % 5 == 0)
            {
                return new Wave
                {
                    Number = waveNumber,
                    EnemyPrefab = BossEnemyPrefab,
                    Count = waveNumber / 5,
                    Frequency = 1,
                    WaveStyle = WaveStyle.Boss,
                };
            }

            return new Wave
            {
                Number = waveNumber,
                EnemyPrefab = EnemyPrefab,
                Count = waveNumber,
                Frequency = 2,
                WaveStyle = WaveStyle.Regular,
            };
        }

        private List<Wave> GetNextWaves(int waveNumber)
        {
            // current wave plus the following 5
            var numbers = Enumerable.Range(waveNumber, 6);
            return numbers.Select(GetWave).ToList();
        }

        public List<GameObject> GetEnemies() => _enemies.Select(e => e.gameObject).ToList();

        public void HandleEnemyEffect(Enemy e, IEffect effect)
        {
            if (effect.Category == EffectCategory.Distract)
            {
                CreateDistractionText(e);
            }
        }

        private void CreateDistractionText(Enemy e)
        {
            var textPos = e.transform.position + new Vector3(0, 0.6f);
            Instantiate(DistractionTextPrefab, textPos, Quaternion.identity, Canvas.transform);
        }

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

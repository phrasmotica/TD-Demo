using TDDemo.Assets.Scripts.Controller;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        public TowerController TowerController { get; set; }

        public TowerManager TowerManager { get; set; }

        public LivesController LivesController { get; set; }

        public MoneyController MoneyController { get; set; }

        public WavesController WavesController { get; set; }

        public GameOverController GameOverController { get; set; }

        public int GetFinalMoney() => MoneyController.Money;

        public void Restart()
        {
            WavesController.StopAllCoroutines();
            WavesController.ResetWaves();
            WavesController.ResetEnemies();

            MoneyController.ResetMoney();

            LivesController.ResetLives();

            TowerController.ResetTowers();

            TowerManager.DeselectCurrentTower();

            GameOverController.RestartGame();
        }
    }
}

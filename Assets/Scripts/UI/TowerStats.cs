using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerStats : MonoBehaviour
    {
        public TowerController TowerController;

        public Text DefaultText;

        public Text NameText;

        public KillCount KillCount;

        public ExperienceBar XpBar;

        public TargetingButtons TargetingButtons;

        private void Awake()
        {
            TowerController.OnStartUpgradeSelectedTower += SetStats;
            TowerController.OnFinishUpgradeSelectedTower += SetStats;
            TowerController.OnLevelChangeSelectedTower += SetStats;

            TowerController.OnSellSelectedTower += tower => ClearStats();

            TowerController.OnChangeSelectedTower += tower =>
            {
                SetStats(tower);

                if (tower != null)
                {
                    XpBar.UpdateProgressBar(tower.Experience);
                    KillCount.UpdateKillCount(tower.KillCount);
                }
            };

            TowerController.OnKillCountChangeTower += (tower, _) =>
            {
                if (tower != null && tower.IsSelected)
                {
                    KillCount.UpdateKillCount(tower.KillCount);
                }
            };

            TowerController.OnXpChangeTower += (tower, _) =>
            {
                if (tower != null && tower.IsSelected)
                {
                    XpBar.UpdateProgressBar(tower.Experience);
                }
            };
        }

        private void Start()
        {
            DefaultText.enabled = true;

            ClearStats();
        }

        public void SetStats(TowerBehaviour tower)
        {
            if (tower != null && tower.IsSelected)
            {
                DefaultText.enabled = false;

                NameText.gameObject.SetActive(true);
                NameText.text = tower.Name;

                XpBar.gameObject.SetActive(true);

                TargetingButtons.gameObject.SetActive(true);

                KillCount.gameObject.SetActive(true);
            }
            else
            {
                DefaultText.enabled = true;

                ClearStats();
            }
        }

        private void ClearStats()
        {
            NameText.gameObject.SetActive(false);
            XpBar.gameObject.SetActive(false);
            TargetingButtons.gameObject.SetActive(false);
            KillCount.gameObject.SetActive(false);
        }
    }
}

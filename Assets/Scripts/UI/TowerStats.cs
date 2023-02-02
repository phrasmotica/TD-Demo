using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Experience;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerStats : MonoBehaviour
    {
        public TowerController TowerController;

        public Text DefaultText;

        public Text NameText;

        public Text DamageText;

        public Text RangeText;

        public Text FireRateText;

        public ExperienceBar XpBar;

        public TargetingButtons TargetingButtons;

        public Text KillCountText;

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
                }
            };

            TowerController.OnKillCountChangeTower += (tower, _) => SetStats(tower);

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

                DamageText.gameObject.SetActive(true);
                DamageText.text = $"Damage: {tower.GetDamage()}";

                RangeText.gameObject.SetActive(true);
                RangeText.text = $"Range: {tower.GetRange()}";

                FireRateText.gameObject.SetActive(true);
                FireRateText.text = $"Fire rate: {tower.GetFireRate()}";

                XpBar.gameObject.SetActive(true);

                TargetingButtons.gameObject.SetActive(true);

                KillCountText.gameObject.SetActive(true);
                KillCountText.text = $"Kills: {tower.KillCount}";
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
            DamageText.gameObject.SetActive(false);
            RangeText.gameObject.SetActive(false);
            FireRateText.gameObject.SetActive(false);
            XpBar.gameObject.SetActive(false);
            TargetingButtons.gameObject.SetActive(false);
            KillCountText.gameObject.SetActive(false);
        }
    }
}

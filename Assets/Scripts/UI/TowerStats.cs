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

        public Text DamageText;

        public Text RangeText;

        public Text FireRateText;

        public Text LevelText;

        public Text XpText;

        private void Awake()
        {
            TowerController.OnStartUpgradeSelectedTower += SetStats;
            TowerController.OnFinishUpgradeSelectedTower += SetStats;
            TowerController.OnChangeSelectedTower += SetStats;
            TowerController.OnLevelChangeSelectedTower += SetStats;
            TowerController.OnXpChangeSelectedTower += (tower, amount) => SetStats(tower);
            TowerController.OnSellSelectedTower += tower => ClearStats();
        }

        public void SetStats(TowerBehaviour tower)
        {
            if (tower != null && tower.IsSelected)
            {
                DefaultText.enabled = false;

                DamageText.gameObject.SetActive(true);
                DamageText.text = $"Damage: {tower.GetDamage()}";

                RangeText.gameObject.SetActive(true);
                RangeText.text = $"Range: {tower.GetRange()}";

                FireRateText.gameObject.SetActive(true);
                FireRateText.text = $"Fire rate: {tower.GetFireRate()}";

                LevelText.gameObject.SetActive(true);
                LevelText.text = $"Level: {tower.Level}";

                XpText.gameObject.SetActive(true);
                XpText.text = $"XP: {tower.CurrentXp}/{tower.NextLevelXp}";
            }
            else
            {
                DefaultText.enabled = true;

                ClearStats();
            }
        }

        private void ClearStats()
        {
            DamageText.gameObject.SetActive(false);
            RangeText.gameObject.SetActive(false);
            FireRateText.gameObject.SetActive(false);
            LevelText.gameObject.SetActive(false);
            XpText.gameObject.SetActive(false);
        }
    }
}

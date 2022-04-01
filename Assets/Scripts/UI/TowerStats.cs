using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerStats : MonoBehaviour
    {
        public TowerController TowerController;

        public TowerManager TowerManager;

        public Text DamageText;

        public Text RangeText;

        public Text FireRateText;

        private void Awake()
        {
            TowerController.OnStartUpgradeSelectedTower += SetStats;
            TowerController.OnFinishUpgradeSelectedTower += SetStats;
            TowerController.OnSellSelectedTower += tower => ClearStats();

            TowerManager.OnSelectedTowerChange += SetStats;
        }

        public void SetStats(TowerBehaviour tower)
        {
            if (tower != null)
            {
                DamageText.gameObject.SetActive(true);
                DamageText.text = $"Damage: {tower.GetDamage()}";

                RangeText.gameObject.SetActive(true);
                RangeText.text = $"Range: {tower.GetRange()}";

                FireRateText.gameObject.SetActive(true);
                FireRateText.text = $"Fire rate: {tower.GetFireRate()}";
            }
            else
            {
                ClearStats();
            }
        }

        private void ClearStats()
        {
            DamageText.gameObject.SetActive(false);
            RangeText.gameObject.SetActive(false);
            FireRateText.gameObject.SetActive(false);
        }
    }
}

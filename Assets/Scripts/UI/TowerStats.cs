using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerStats : MonoBehaviour
    {
        private Text _damageText;
        private Text _rangeText;
        private Text _fireRateText;

        private void Start()
        {
            _damageText = transform.Find("DamageText").GetComponent<Text>();
            _rangeText = transform.Find("RangeText").GetComponent<Text>();
            _fireRateText = transform.Find("FireRateText").GetComponent<Text>();
        }

        public void SetStats(TowerBehaviour tower)
        {
            if (tower != null)
            {
                var towerLevel = tower.GetLevel();

                _damageText.gameObject.SetActive(true);
                _damageText.text = $"Damage: {towerLevel.Damage}";

                _rangeText.gameObject.SetActive(true);
                _rangeText.text = $"Range: {towerLevel.Range}";

                _fireRateText.gameObject.SetActive(true);
                _fireRateText.text = $"Fire rate: {towerLevel.FireRate}";
            }
            else
            {
                _damageText.gameObject.SetActive(false);
                _rangeText.gameObject.SetActive(false);
                _fireRateText.gameObject.SetActive(false);
            }
        }
    }
}

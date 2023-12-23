using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject Tooltip;

        public Button Button;

        public int UpgradeIndex;

        private UpgradeNode _upgrade;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Button.interactable)
            {
                Tooltip.SetActive(true);
            }

            var tooltip = Tooltip.GetComponent<UpgradeTooltip>();

            // by this point the Awake() methods of any children of _currentTooltip
            // have been called, so it is safe to do this
            tooltip.SetUpgrade(_upgrade);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.SetActive(false);
        }

        public void SetUpgradeFromTower(TowerBehaviour tower)
        {
            if (tower != null)
            {
                (_, _upgrade) = tower.GetUpgradeInfo(UpgradeIndex);
            }
            else
            {
                _upgrade = null;
            }
        }
    }
}

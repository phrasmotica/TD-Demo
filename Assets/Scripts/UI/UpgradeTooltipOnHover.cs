using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTooltipOnHover : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject Tooltip;

        public Button Button;

        public int UpgradeIndex;

        private bool _mouseIsOver;

        private TowerBehaviour _tower;

        private void Start()
        {
            logger = new(nameof(UpgradeTooltipOnHover));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseIsOver = true;

            if (Button.interactable)
            {
                Tooltip.SetActive(true);
            }

            RefreshTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseIsOver = false;

            Tooltip.SetActive(false);
        }

        public void SetUpgradeFromTower(TowerBehaviour tower)
        {
            _tower = tower;

            RefreshTooltip();
        }

        private void RefreshTooltip()
        {
            if (_mouseIsOver)
            {
                var (_, upgrade) = ComputeUpgrade();

                Tooltip.SetActive(Button.interactable && upgrade != null);

                var tooltip = Tooltip.GetComponent<UpgradeTooltip>();
                tooltip.SetUpgrade(upgrade);
            }
        }

        private (bool, UpgradeNode) ComputeUpgrade()
        {
            if (_tower != null)
            {
                return _tower.GetUpgradeInfo(UpgradeIndex);
            }
            
            return (false, null);
        }
    }
}

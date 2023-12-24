using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTooltipOnHover : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject Tooltip;

        public Button Button;

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

        public void SetTower(TowerBehaviour tower)
        {
            _tower = tower;

            RefreshTooltip();
        }

        private void RefreshTooltip()
        {
            if (_mouseIsOver)
            {
                Tooltip.SetActive(Button.interactable);

                var tooltip = Tooltip.GetComponent<SellTooltip>();
                tooltip.SetTower(_tower);
            }
        }
    }
}

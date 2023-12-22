using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Canvas UiCanvas;

        public GameObject TooltipPrefab;

        public Button Button;

        public UpgradeNode Upgrade;

        public int UpgradeIndex;

        private GameObject _currentTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            // TODO: make all instances of this script share ONE tooltip object
            if (_currentTooltip == null && Button.interactable)
            {
                _currentTooltip = Instantiate(TooltipPrefab, UiCanvas.transform);
            }

            _currentTooltip.SetActive(true);

            var tooltip = _currentTooltip.GetComponent<UpgradeTooltip>();

            // by this point the Awake() methods of any children of _currentTooltip
            // have been called, so it is safe to do this
            tooltip.SetUpgrade(Upgrade);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_currentTooltip != null)
            {
                _currentTooltip.SetActive(false);
            }
        }
    }
}

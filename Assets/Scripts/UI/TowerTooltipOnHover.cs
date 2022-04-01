using UnityEngine;
using UnityEngine.EventSystems;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Canvas UiCanvas;

        public GameObject TooltipPrefab;

        private GameObject _currentTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentTooltip = Instantiate(TooltipPrefab, UiCanvas.transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(_currentTooltip);
            _currentTooltip = null;
        }
    }
}

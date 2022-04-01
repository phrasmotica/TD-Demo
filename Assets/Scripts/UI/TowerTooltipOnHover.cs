using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Canvas UiCanvas;

        public GameObject TooltipPrefab;

        public TowerBehaviour Tower;

        private GameObject _currentTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentTooltip = Instantiate(TooltipPrefab, UiCanvas.transform);

            var tooltip = _currentTooltip.GetComponent<TowerTooltip>();

            // by this point the Awake() methods of any children of _currentTooltip
            // have been called, so it is safe to do this
            tooltip.SetTower(Tower);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(_currentTooltip);
            _currentTooltip = null;
        }
    }
}

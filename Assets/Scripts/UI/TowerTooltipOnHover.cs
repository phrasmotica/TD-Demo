using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject Tooltip;

        public TowerBehaviour Tower;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.SetActive(true);

            var tooltip = Tooltip.GetComponent<TowerTooltip>();

            // by this point the Awake() methods of any children of _currentTooltip
            // have been called, so it is safe to do this
            tooltip.SetTower(Tower);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.SetActive(false);
        }
    }
}

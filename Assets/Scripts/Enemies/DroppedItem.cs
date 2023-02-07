using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class DroppedItem : MonoBehaviour
    {
        private bool _isMouseOver;

        public PickupRouter PickupRouter { get; set; }

        public event UnityAction OnMouseEnterEvent;

        public event UnityAction OnMouseExitEvent;

        private void Update()
        {
            if (_isMouseOver && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                // TODO: allow attaching and picking up multiple items
                var pickupItem = GetComponent<IPickupItem>();
                pickupItem.Pickup(PickupRouter);

                Destroy(gameObject);
            }
        }

        private void OnMouseEnter()
        {
            _isMouseOver = true;
            OnMouseEnterEvent?.Invoke();
        }

        private void OnMouseExit()
        {
            _isMouseOver = false;
            OnMouseExitEvent?.Invoke();
        }
    }
}

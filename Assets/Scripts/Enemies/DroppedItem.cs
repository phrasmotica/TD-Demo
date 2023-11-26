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

        public UnityEvent OnMouseEnterEvent;

        public UnityEvent OnMouseExitEvent;

        private void Update()
        {
            if (_isMouseOver && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                var pickupItem = GetComponent<IPickupItem>();
                pickupItem.Pickup(PickupRouter);

                Destroy(gameObject);
            }
        }

        private void OnMouseEnter()
        {
            _isMouseOver = true;
            OnMouseEnterEvent.Invoke();
        }

        private void OnMouseExit()
        {
            _isMouseOver = false;
            OnMouseExitEvent.Invoke();
        }
    }
}

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

        //public UnityEvent<IPickupItem> OnPickup;

        private void Update()
        {
            if (_isMouseOver && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                var pickupItem = GetComponent<IPickupItem>();

                // TODO: probably there's a more sensible way to pick up the items... currently they
                // pick themselves up and pass themselves to the pickup router. Should really
                // be the other way around.
                // Why is this causing an exception that I can't track down?
                // https://forum.unity.com/threads/object-of-type-unityengine-object-cannot-be-converted-to-type-unityengine-game.1186891/
                //OnPickup.Invoke(pickupItem);

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

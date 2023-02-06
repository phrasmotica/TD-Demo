using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class CouponDrop : MonoBehaviour
    {
        [Range(1, 2)]
        public int Bounty;

        private bool _isMouseOver;

        public BankManager Bank { get; set; }

        public event UnityAction OnMouseEnterEvent;

        public event UnityAction OnMouseExitEvent;

        private void Update()
        {
            if (_isMouseOver && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                Bank.AddCoupons(Bounty);

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

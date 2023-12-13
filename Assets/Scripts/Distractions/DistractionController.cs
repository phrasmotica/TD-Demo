using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Distractions
{
    public class DistractionController : MonoBehaviour
    {
        private DistractionSource _newDistraction;

        public BankManager BankManager;

        public UnityEvent<DistractionSource> OnPlaceDistraction;

        private void Update()
        {
            if (_newDistraction != null && _newDistraction.IsPositioning)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    CancelCreateDistraction();
                    return;
                }

                if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                {
                    PlaceDistraction();
                    return;
                }

                _newDistraction.transform.FollowMouse();
            }
        }

        public void CreateNewDistraction(GameObject distractionPrefab)
        {
            // only create if we can afford the tower
            var distraction = distractionPrefab.GetComponent<DistractionSource>();

            var canBuy = BankManager.CanAfford(distraction.Price) != PurchaseMethod.None;
            if (canBuy)
            {
                _newDistraction = Instantiate(distractionPrefab).GetComponent<DistractionSource>();
            }
        }

        private void PlaceDistraction()
        {
            if (_newDistraction != null)
            {
                OnPlaceDistraction.Invoke(_newDistraction);

                _newDistraction.Place();
                _newDistraction = null;
            }
        }

        public void CancelCreateDistraction()
        {
            if (_newDistraction != null)
            {
                Destroy(_newDistraction.gameObject);

                _newDistraction = null;
            }
        }
    }
}

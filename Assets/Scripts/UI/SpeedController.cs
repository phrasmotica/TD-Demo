using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.UI
{
    public class SpeedController : MonoBehaviour
    {
        private const int FastForwardSpeed = 3;

        private bool _isOn;

        public UnityEvent OnFastForward;

        public UnityEvent OnNormalSpeed;

        public void ToggleFastForward()
        {
            _isOn = !_isOn;

            Time.timeScale = _isOn ? FastForwardSpeed : 1;

            if (_isOn)
            {
                OnFastForward.Invoke();
            }
            else
            {
                OnNormalSpeed.Invoke();
            }
        }
    }
}

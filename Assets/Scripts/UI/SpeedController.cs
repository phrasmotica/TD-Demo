using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.UI
{
    public class SpeedController : MonoBehaviour
    {
        private const float FastForwardSpeed = 3;

        private bool _isPaused;

        private bool _isFastForward;

        private float _lastSpeed;

        public UnityEvent OnPause;

        public UnityEvent OnResume;

        public UnityEvent OnFastForward;

        public UnityEvent OnNormalSpeed;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Debug.Log("Paused");
                _lastSpeed = Time.timeScale;
                Time.timeScale = 0;
                OnPause.Invoke();
            }
            else
            {
                Debug.Log("Resumed");
                Time.timeScale = _lastSpeed;
                OnResume.Invoke();
            }
        }

        public void ToggleFastForward()
        {
            _isFastForward = !_isFastForward;

            if (_isFastForward)
            {
                Debug.Log($"Fast forward x{FastForwardSpeed}");
                Time.timeScale = FastForwardSpeed;
                OnFastForward.Invoke();
            }
            else
            {
                Debug.Log("Normal speed");
                Time.timeScale = 1;
                OnNormalSpeed.Invoke();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.UI
{
    public class SpeedController : MonoBehaviour
    {
        private const float FastForwardSpeed = 3;

        private bool _isPaused;

        private bool _isFastForward;

        private float _currentSpeed;

        public UnityEvent OnPause;

        public UnityEvent OnResume;

        public UnityEvent OnFastForward;

        public UnityEvent OnNormalSpeed;

        private void Start()
        {
            _currentSpeed = Time.timeScale;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                TogglePause();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ToggleFastForward();
            }
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Time.timeScale = 0;

                Debug.Log("Paused");
                OnPause.Invoke();
            }
            else
            {
                Time.timeScale = _currentSpeed;

                Debug.Log("Resumed");
                OnResume.Invoke();
            }
        }

        public void ToggleFastForward()
        {
            _isFastForward = !_isFastForward;

            if (_isFastForward)
            {
                _currentSpeed = FastForwardSpeed;
                
                Debug.Log($"Fast forward x{FastForwardSpeed}");
                OnFastForward.Invoke();

                if (!_isPaused)
                {
                    Time.timeScale = FastForwardSpeed;
                }
            }
            else
            {
                _currentSpeed = 1;

                Debug.Log("Normal speed");
                OnNormalSpeed.Invoke();

                if (!_isPaused)
                {
                    Time.timeScale = 1;
                }
            }
        }
    }
}

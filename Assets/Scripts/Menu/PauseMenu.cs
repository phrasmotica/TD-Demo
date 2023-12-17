using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TDDemo
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseMenuUI;

        public TowerController TowerController;

        private bool _gameIsPaused = false;

        private float _currentTimeScale = 1f;

        public UnityEvent OnGamePaused;

        public UnityEvent OnGameResumed;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !TowerController.IsPositioning && !TowerController.IsSelected)
            {
                if (_gameIsPaused)
                {
                    Resume();

                    OnGameResumed.Invoke();
                }
                else
                {
                    Pause();

                    OnGamePaused.Invoke();
                }
            }
        }

        public void Resume()
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = _currentTimeScale;
            _gameIsPaused = false;
        }

        private void Pause()
        {
            PauseMenuUI.SetActive(true);
            _currentTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            _gameIsPaused = true;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}

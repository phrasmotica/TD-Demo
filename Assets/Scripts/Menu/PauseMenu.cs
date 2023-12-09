using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TDDemo
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;

        public static float CurrentTimeScale = 1f;

        public GameObject PauseMenuUI;

        public UnityEvent OnGamePaused;

        public UnityEvent OnGameResumed;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
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
            Time.timeScale = CurrentTimeScale;
            GameIsPaused = false;
        }

        private void Pause()
        {
            PauseMenuUI.SetActive(true);
            CurrentTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            GameIsPaused = true;
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

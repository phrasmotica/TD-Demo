using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDDemo.Assets.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}

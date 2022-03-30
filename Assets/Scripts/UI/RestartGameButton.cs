using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class RestartGameButton : MonoBehaviour
    {
        public GameOver GameOver;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(GameOver.Restart);
        }
    }
}

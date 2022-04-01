using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class LivesText : MonoBehaviour
    {
        public LivesController LivesController;

        private void Awake()
        {
            LivesController.OnLivesChange += lives =>
            {
                var text = GetComponent<Text>();
                text.text = $"Lives: {lives}";
            };
        }
    }
}

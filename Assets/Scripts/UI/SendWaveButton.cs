using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SendWaveButton : MonoBehaviour
    {
        public WavesController WavesController;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(WavesController.DoSendNextWave);

            WavesController.OnWaveChange += wave =>
            {
                var text = GetComponentInChildren<Text>();
                text.text = $"Send wave {wave + 1}";
            };
        }
    }
}

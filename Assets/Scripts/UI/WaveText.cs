using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class WaveText : MonoBehaviour
    {
        public WavesController WavesController;

        private void Start()
        {
            WavesController.OnWaveChange += wave =>
            {
                var text = GetComponent<Text>();
                text.text = $"Wave {wave}";
            };
        }
    }
}

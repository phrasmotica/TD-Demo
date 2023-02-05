using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class WaveText : MonoBehaviour
    {
        public WavesController WavesController;

        private void Awake()
        {
            WavesController.OnWaveChange += wave =>
            {
                var text = GetComponent<Text>();
                text.text = $"{wave}";
            };
        }
    }
}

using TDDemo.Assets.Scripts.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class WaveText : MonoBehaviour
    {
        public void UpdateText(int waveNumber, Wave wave)
        {
            var text = GetComponent<Text>();
            text.text = $"{waveNumber}";
        }
    }
}

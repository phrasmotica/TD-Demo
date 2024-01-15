using UnityEngine;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WavePanel : MonoBehaviour
    {
        public WaveTile[] WaveTiles;

        public void UpdateText(int waveNumber, Wave wave)
        {
            foreach (var waveTile in WaveTiles)
            {
                waveTile.UpdateText(waveNumber, wave);
            }
        }
    }
}

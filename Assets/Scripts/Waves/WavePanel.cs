using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WavePanel : MonoBehaviour
    {
        public Button NextWaveButton;

        public WaveTile[] WaveTiles;

        public void SetWaves(List<Wave> nextWaves)
        {
            var futureWaves = nextWaves.Skip(1).ToList();

            NextWaveButton.interactable = futureWaves.Any();

            for (var i = 0; i < WaveTiles.Length; i++)
            {
                var waveTile = WaveTiles[i];
                var wave = i < futureWaves.Count ? futureWaves[i] : null;

                waveTile.SetWave(wave);
            }
        }
    }
}

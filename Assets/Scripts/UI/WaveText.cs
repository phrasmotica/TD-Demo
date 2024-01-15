using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class WaveText : MonoBehaviour
    {
        public void SetWave(List<Wave> nextWaves)
        {
            if (nextWaves.Any())
            {
                var text = GetComponent<Text>();
                text.text = $"{nextWaves[0].Number}";
            }
        }
    }
}

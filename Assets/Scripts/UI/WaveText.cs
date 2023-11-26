using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class WaveText : MonoBehaviour
    {
        public void UpdateText(int wave)
        {
            var text = GetComponent<Text>();
            text.text = $"{wave}";
        }
    }
}

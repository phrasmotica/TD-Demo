using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class LivesText : MonoBehaviour
    {
        public void UpdateText(int lives)
        {
            var text = GetComponent<Text>();
            text.text = $"{lives}";
        }
    }
}

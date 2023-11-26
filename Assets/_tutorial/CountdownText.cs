using TMPro;
using UnityEngine;

namespace TDDemo
{
    public class CountdownText : MonoBehaviour
    {
        public TMP_Text Text;

        public void SetText(float countdown)
        {
            Text.text = $"{Mathf.Ceil(countdown)}";
        }
    }
}

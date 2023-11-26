using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MoneyText : MonoBehaviour
    {
        public void UpdateText(int money)
        {
            var text = GetComponent<Text>();
            text.text = $"{money}";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class CouponsText : MonoBehaviour
    {
        public void UpdateText(int coupons)
        {
            var text = GetComponent<Text>();
            text.text = $"{coupons}";
        }
    }
}

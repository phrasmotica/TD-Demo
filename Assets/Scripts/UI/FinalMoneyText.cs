using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class FinalMoneyText : MonoBehaviour
    {
        public GameOver GameOver;

        private void Start()
        {
            var text = GetComponent<Text>();
            text.text = $"You finished with {GameOver.GetFinalMoney()} money.";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SpeedUpToggle : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(SetTimeScale);
        }

        private void SetTimeScale(bool speedUp)
        {
            Time.timeScale = speedUp ? 3 : 1;
        }
    }
}

using TDDemo.Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SendWaveButton : MonoBehaviour
    {
        public WavesController WavesController;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(WavesController.DoSendNextWave);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MuteToggle : MonoBehaviour
    {
        public AudioSource MusicSource;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(SetVolume);
        }

        private void SetVolume(bool mute) => MusicSource.mute = mute;
    }
}

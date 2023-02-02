using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MuteToggle : MonoBehaviour
    {
        // TODO: change this to a list of audio sources, and mute them all
        public AudioSource MusicSource;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(SetVolume);
        }

        private void SetVolume(bool mute) => MusicSource.mute = mute;
    }
}

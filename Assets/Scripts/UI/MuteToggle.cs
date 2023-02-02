using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MuteToggle : MonoBehaviour
    {
        // TODO: add audio sources to this list as they're created
        // and remove them once they're gone
        public List<AudioSource> AudioSources;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(SetVolume);
        }

        private void SetVolume(bool mute)
        {
            AudioSources.ForEach(s => s.mute = mute);
        }
    }
}

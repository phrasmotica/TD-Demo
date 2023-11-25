using UnityEngine;
using UnityEngine.Audio;

namespace TDDemo.Assets.Scripts.Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
        }
    }
}

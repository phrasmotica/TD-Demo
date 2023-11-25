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

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
    }
}

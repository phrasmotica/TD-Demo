using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public Slider volumeSlider;

        public TMP_Dropdown resolutionDropdown;

        private List<Resolution> _resolutions;

        private void Start()
        {
            audioMixer.GetFloat("masterVolume", out var currentVolume);

            Debug.Log($"Current volume is {currentVolume}");

            volumeSlider.value = currentVolume;

            _resolutions = Screen.resolutions.ToList();

            var distinctRefreshRates = _resolutions.Select(r => r.refreshRateRatio.value).Distinct();
            var showRefreshRates = distinctRefreshRates.Count() > 1;

            resolutionDropdown.ClearOptions();

            resolutionDropdown.AddOptions(_resolutions.Select(r => new TMP_Dropdown.OptionData
            {
                text = showRefreshRates ? 
                    $"{r.width} x {r.height} {r.refreshRateRatio.value}Hz" :
                    $"{r.width} x {r.height}",
            }).ToList());

            var currentIndex = _resolutions.FindIndex(r => 
                r.width == Screen.currentResolution.width && 
                r.height == Screen.currentResolution.height);

            resolutionDropdown.value = currentIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("masterVolume", volume);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetResolution(int resolutionIndex)
        {
            var resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
    }
}

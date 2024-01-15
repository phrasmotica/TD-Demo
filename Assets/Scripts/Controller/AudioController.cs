using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Waves;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class AudioController : MonoBehaviour
    {
        public AudioMixer AudioMixer;

        private bool _isMuted;

        private readonly Dictionary<string, float> _currentVolumes = new();

        public UnityEvent OnMute;

        public UnityEvent OnUnmute;

        private void Start()
        {
            ResetAudio();
        }

        public void Toggle()
        {
            _isMuted = !_isMuted;

            if (_isMuted)
            {
                AudioMixer.GetFloat("masterVolume", out var masterVolume);
                _currentVolumes["masterVolume"] = masterVolume;
                AudioMixer.SetFloat("masterVolume", -80);

                AudioMixer.GetFloat("musicVolume", out var musicVolume);
                _currentVolumes["musicVolume"] = musicVolume;
                AudioMixer.SetFloat("musicVolume", -80);

                AudioMixer.GetFloat("soundEffectsVolume", out var soundEffectsVolume);
                _currentVolumes["soundEffectsVolume"] = soundEffectsVolume;
                AudioMixer.SetFloat("soundEffectsVolume", -80);

                OnMute.Invoke();
            }
            else
            {
                AudioMixer.SetFloat("masterVolume", _currentVolumes["masterVolume"]);
                AudioMixer.SetFloat("musicVolume", _currentVolumes["musicVolume"]);
                AudioMixer.SetFloat("soundEffectsVolume", _currentVolumes["soundEffectsVolume"]);

                OnUnmute.Invoke();
            }
        }

        public void ResetAudio()
        {
            AudioMixer.SetFloat("musicPitch", 1f);
        }

        public void SetMusicForCurrentWave(List<Wave> nextWaves)
        {
            if (!nextWaves.Any())
            {
                return;
            }

            if (nextWaves[0].WaveStyle == WaveStyle.Boss)
            {
                AudioMixer.SetFloat("musicPitch", 0.9f);
            }
            else
            {
                AudioMixer.SetFloat("musicPitch", 1f);
            }
        }
    }
}
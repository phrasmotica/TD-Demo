using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TDDemo.Assets.Scripts.UI
{
    public class MuteButton : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public Sprite MutedSprite;

        public Sprite UnmutedSprite;

        public AudioMixer AudioMixer;

        private bool _isMuted;

        private readonly Dictionary<string, float> _currentVolumes = new();

        public void Toggle()
        {
            _isMuted = !_isMuted;

            // TODO: move all this into an AudioController script
            if (_isMuted)
            {
                AudioMixer.GetFloat("masterVolume", out var masterVolume);
                _currentVolumes["masterVolume"] = masterVolume;
                AudioMixer.SetFloat("masterVolume", -80);

                AudioMixer.GetFloat("musicVolume", out var musicVolume);
                _currentVolumes["musicVolume"] = musicVolume;
                AudioMixer.SetFloat("musicVolume", -80);

                SpriteRenderer.sprite = MutedSprite;
            }
            else
            {
                AudioMixer.SetFloat("masterVolume", _currentVolumes["masterVolume"]);
                AudioMixer.SetFloat("musicVolume", _currentVolumes["musicVolume"]);

                SpriteRenderer.sprite = UnmutedSprite;
            }
        }
    }
}

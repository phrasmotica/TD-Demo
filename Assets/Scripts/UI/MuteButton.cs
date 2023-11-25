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

        private float _currentVolume;

        public void Toggle()
        {
            _isMuted = !_isMuted;

            if (_isMuted)
            {
                AudioMixer.GetFloat("volume", out _currentVolume);
                AudioMixer.SetFloat("volume", -80);
                SpriteRenderer.sprite = MutedSprite;
            }
            else
            {
                AudioMixer.SetFloat("volume", _currentVolume);
                SpriteRenderer.sprite = UnmutedSprite;
            }
        }
    }
}

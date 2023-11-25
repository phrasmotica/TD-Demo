using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MuteButton : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public Sprite EnabledSprite;

        public Sprite DisabledSprite;

        public AudioMixer AudioMixer;

        private bool _isOn;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Toggle);
        }

        private void Toggle()
        {
            _isOn = !_isOn;

            // TODO: reset to whatever the current volume is, rather than always 0
            AudioMixer.SetFloat("volume", _isOn ? -80 : 0); // feel like these two values should be swapped?

            SpriteRenderer.sprite = _isOn ? EnabledSprite : DisabledSprite;
        }
    }
}

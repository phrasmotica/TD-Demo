using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class MuteButton : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public Sprite EnabledSprite;

        public Sprite DisabledSprite;

        // TODO: add audio sources to this list as they're created
        // and remove them once they're gone
        public List<AudioSource> AudioSources;

        private bool _isOn;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Toggle);
        }

        private void Toggle()
        {
            _isOn = !_isOn;

            AudioSources.ForEach(s => s.mute = _isOn);

            SpriteRenderer.sprite = _isOn ? EnabledSprite : DisabledSprite;
        }
    }
}

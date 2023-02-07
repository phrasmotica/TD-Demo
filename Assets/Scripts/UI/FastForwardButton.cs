using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class FastForwardButton : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public Sprite EnabledSprite;
        
        public Sprite DisabledSprite;

        private bool _isOn;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Toggle);
        }

        private void Toggle()
        {
            _isOn = !_isOn;

            Time.timeScale = _isOn ? 3 : 1;

            SpriteRenderer.sprite = _isOn ? EnabledSprite : DisabledSprite;
        }
    }
}

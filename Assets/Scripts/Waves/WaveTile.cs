using TDDemo.Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WaveTile : MonoBehaviour
    {
        private Wave _wave;

        public Image BackgroundImage;

        public Image EnemyImage;

        public TMP_Text AmountText;

        public TMP_Text WaveNumberText;

        public Button SendWaveButton;

        public Image SendWaveButtonImage;

        public UnityEvent<Wave> OnSendWave;

        public void SetWave(Wave wave)
        {
            _wave = wave;

            if (wave != null)
            {
                BackgroundImage.color = wave.WaveStyle switch
                {
                    WaveStyle.Boss => ColourHelper.BossWave,
                    _ => ColourHelper.DefaultWave,
                };

                var spriteRenderer = wave.EnemyPrefab.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    EnemyImage.sprite = spriteRenderer.sprite;
                }

                AmountText.text = $"x{wave.Count}";
                WaveNumberText.text = $"{wave.Number}";

                SendWaveButton.interactable = true;
                SendWaveButtonImage.color = Color.white;
            }
            else
            {
                BackgroundImage.color = ColourHelper.NoWave;
                EnemyImage.sprite = null;
                AmountText.text = string.Empty;
                WaveNumberText.text = "?";

                SendWaveButton.interactable = false;
                SendWaveButtonImage.color = ColourHelper.InteractionDisabled;
            }
        }

        public void SendWave()
        {
            if (_wave != null)
            {
                OnSendWave.Invoke(_wave);
            }
        }
    }
}

using TDDemo.Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WaveTile : MonoBehaviour
    {
        public Image BackgroundImage;

        public Image EnemyImage;

        public TMP_Text AmountText;

        public TMP_Text WaveNumberText;

        public void UpdateText(int waveNumber, Wave wave)
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
            WaveNumberText.text = $"{waveNumber}";
        }
    }
}

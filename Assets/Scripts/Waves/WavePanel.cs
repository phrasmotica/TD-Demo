using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.Waves
{
    public class WavePanel : MonoBehaviour
    {
        public Image EnemyImage;

        public TMP_Text AmountText;

        public void UpdateText(int waveNumber, Wave wave)
        {
            var spriteRenderer = wave.EnemyPrefab.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                EnemyImage.sprite = spriteRenderer.sprite;
            }

            AmountText.text = $"x{wave.Count}";
        }
    }
}

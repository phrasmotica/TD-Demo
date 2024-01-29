using System.Collections;
using TDDemo.Assets.Scripts.Towers;
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

        private TimeCounter _timeCounter;

        private float _progressBarWidth;

        private Coroutine _progressCoroutine;

        public Image BackgroundImage;

        public Image ProgressImage;

        public Image EnemyImage;

        public TMP_Text AmountText;

        public TMP_Text WaveNumberText;

        public Button SendWaveButton;

        public Image SendWaveButtonImage;

        public bool ShowProgress;

        public UnityEvent<Wave> OnSendWave;

        private void Awake()
        {
            // https://discussions.unity.com/t/how-do-i-get-the-literal-width-of-a-recttransform/135984/2
            _progressBarWidth = ProgressImage.rectTransform.rect.width;
        }

        public void SetWave(Wave wave)
        {
            _wave = wave;

            _timeCounter?.Stop();
            SetProgress(0);

            if (_progressCoroutine is not null)
            {
                StopCoroutine(_progressCoroutine);
            }

            if (wave != null)
            {
                BackgroundImage.color = wave.WaveStyle switch
                {
                    WaveStyle.Boss => ColourHelper.BossWave,
                    _ => ColourHelper.DefaultWave,
                };

                if (ShowProgress)
                {
                    _progressCoroutine = StartCoroutine(AnimateProgress(wave.DelaySeconds));
                }

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

        private IEnumerator AnimateProgress(float timeSeconds)
        {
            _timeCounter = new(timeSeconds);
            _timeCounter.Start();

            while (!_timeCounter.IsFinished)
            {
                _timeCounter.Increment(Time.deltaTime);
                SetProgress(_timeCounter.Progress);
                yield return null;
            }
        }

        private void SetProgress(float progress)
        {
            // ensure progress bar has zero width, achieved via a "right" value
            // equal to the width of the bar. offsetMax requires the "right" value
            // to be passed as negative:
            // https://discussions.unity.com/t/access-left-right-top-and-bottom-of-recttransform-via-script/129237/3

            var width = _progressBarWidth * (1 - progress);
            ProgressImage.rectTransform.offsetMax = new(-width, 0);
        }
    }
}

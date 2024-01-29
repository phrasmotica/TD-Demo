using System;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Waves
{
    [Serializable]
    public class Wave
    {
        public int Number { get; set; }

        public GameObject EnemyPrefab;

        public int Count;

        public int Frequency;

        public WaveStyle WaveStyle;

        /// <summary>
        /// The time that should pass BEFORE this wave is sent, i.e. the time since the last wave
        /// was sent. Does not apply to the first wave.
        /// </summary>
        [Range(5f, 10f)]
        public float DelaySeconds;
    }

    public enum WaveStyle
    {
        Regular,
        Boss,
    }
}

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
    }

    public enum WaveStyle
    {
        Regular,
        Boss,
    }
}

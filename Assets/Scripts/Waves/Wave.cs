using System;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Waves
{
    [Serializable]
    public class Wave
    {
        public GameObject EnemyPrefab;

        public int Count;

        public int Frequency;
    }
}

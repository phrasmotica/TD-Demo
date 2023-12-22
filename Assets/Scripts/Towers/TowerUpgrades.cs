using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo
{
    public class TowerUpgrades : BaseBehaviour
    {
        public List<TowerLevel> Levels;

        public int Count => Levels.Count;

        private void Start()
        {
            logger = new(nameof(BaseBehaviour));
        }

        public void ToggleLevelObjects(TowerBehaviour tower, int level)
        {
            // make sure only the current level object is active
            for (var i = 0; i < Levels.Count; i++)
            {
                var levelObj = Levels[i];
                levelObj.gameObject.SetActive(i == level);
            }
        }

        public Sprite GetSprite(int level)
        {
            return Levels[level].GetComponent<SpriteRenderer>().sprite;
        }

        public int GetPrice() => Levels.First().Price;

        public int GetTotalValue(int level) => Levels.Take(level + 1).Sum(l => l.Price);

        public float GetWarmupTime() => GetUpgradeTime(0);

        public float GetUpgradeTime(int level)
        {
            var nextLevel = Levels[level + 1];
            return nextLevel.Time;
        }

        public TowerLevel GetNextLevel(int level)
        {
            var isMaxLevel = level >= Count - 1;
            if (isMaxLevel)
            {
                return null;
            }

            return Levels[level + 1];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Upgrades
{
    public class UpgradeNode : MonoBehaviour
    {
        public string Name;

        public Sprite Sprite;

        public string Description;

        [Range(1, 10)]
        public int Price;

        [Range(0.5f, 10f)]
        public float Time;

        public List<UpgradeNode> Upgrades;

        /// <summary>
        /// Returns the tower level at the end of the path indicated by the array of indexes.
        /// </summary>
        public UpgradeNode GetEnd(int[] pathIndexes)
        {
            if (pathIndexes is null || !pathIndexes.Any())
            {
                return this;
            }

            if (Upgrades.Count < pathIndexes[0] - 1)
            {
                throw new IndexOutOfRangeException($"No upgrade index={pathIndexes[0]} exists, only have {Upgrades.Count} of them");
            }

            return Upgrades[pathIndexes[0]].GetEnd(pathIndexes[1..]);
        }

        /// <summary>
        /// Returns all tower levels in the path indicated by the array of indexes.
        /// </summary>
        public UpgradePath GetPath(int[] pathIndexes)
        {
            var path = new UpgradePath(this);

            if (pathIndexes is null || !pathIndexes.Any())
            {
                return path;
            }

            if (Upgrades.Count < pathIndexes[0] - 1)
            {
                throw new IndexOutOfRangeException($"No upgrade index={pathIndexes[0]} exists, only have {Upgrades.Count} of them");
            }

            path.Append(Upgrades[pathIndexes[0]].GetPath(pathIndexes[1..]));

            return path;
        }

        /// <summary>
        /// Returns all tower levels in the tree.
        /// </summary>
        public List<UpgradeNode> GetAll()
        {
            var levels = new List<UpgradeNode> { this };

            foreach (var level in Upgrades)
            {
                levels.AddRange(level.GetAll());
            }

            return levels;
        }
    }
}

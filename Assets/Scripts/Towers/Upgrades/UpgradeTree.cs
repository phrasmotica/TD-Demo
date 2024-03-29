using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Util;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers.Upgrades
{
    public class UpgradeTree : BaseBehaviour
    {
        public UpgradeNode RootNode;

        private readonly List<int> _nodeIndexes = new();

        public UnityEvent<UpgradePath> OnUpgrade;

        private void Start()
        {
            logger = new(nameof(BaseBehaviour));
        }

        public void Upgrade(int nodeIndex)
        {
            _nodeIndexes.Add(nodeIndex);

            var current = GetCurrent();

            foreach (var l in RootNode.GetAll())
            {
                l.gameObject.SetActive(l == current);
            }

            var path = GetPath();
            OnUpgrade.Invoke(path);
        }

        public UpgradeNode GetCurrent() => RootNode.GetEnd(_nodeIndexes.ToArray());

        public UpgradePath GetPath() => RootNode.GetPath(_nodeIndexes.ToArray());

        public int GetPrice() => RootNode.Price;

        public float GetWarmupTime() => RootNode.Time;

        public float GetUpgradeTime(int index) => GetUpgrades()[index].Time;

        public int GetTotalValue() => GetPath().GetValue();

        public List<UpgradeNode> GetUpgrades() => GetCurrent().Upgrades;

        public bool CanUpgrade() => GetUpgrades().Any();
    }
}

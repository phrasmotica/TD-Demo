using System.Collections.Generic;
using System.Linq;

namespace TDDemo.Assets.Scripts.Towers.Upgrades
{
    /// <summary>
    /// Represents a single path through an upgrade tree, starting with the root node.
    /// </summary>
    public class UpgradePath
    {
        public List<UpgradeNode> Nodes;

        public UpgradePath(UpgradeNode root)
        {
            Nodes = new() { root };
        }

        public void Append(UpgradePath other)
        {
            Nodes.AddRange(other.Nodes);
        }

        public bool Contains(UpgradeNode node) => Nodes.Contains(node);

        public int GetValue() => Nodes.Sum(l => l.Price);
    }
}

using TDDemo.Assets.Scripts.Towers.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeNameText : MonoBehaviour
    {
        public Text Text;

        public void UpdateText(UpgradeNode upgrade)
        {
            if (upgrade != null)
            {
                Text.text = upgrade.Name;
            }
            else
            {
                Text.text = string.Empty;
            }
        }
    }
}

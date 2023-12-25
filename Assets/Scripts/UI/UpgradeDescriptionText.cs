using TDDemo.Assets.Scripts.Towers.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeDescriptionText : MonoBehaviour
    {
        public Text Text;

        public void UpdateText(UpgradeNode upgrade)
        {
            if (upgrade != null)
            {
                Text.text = upgrade.Description;
            }
            else
            {
                Text.text = string.Empty;
            }
        }
    }
}

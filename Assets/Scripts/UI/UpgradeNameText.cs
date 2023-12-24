using TDDemo.Assets.Scripts.Towers.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeNameText : MonoBehaviour
    {
        public void UpdateText(UpgradeNode upgrade)
        {
            if (upgrade != null)
            {
                GetComponent<Text>().text = upgrade.Name;
            }
            else
            {
                GetComponent<Text>().text = string.Empty;
            }
        }
    }
}

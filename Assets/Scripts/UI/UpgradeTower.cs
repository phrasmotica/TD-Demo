using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTower : MonoBehaviour
    {
        public void SetState(bool canUpgradeTower, int? upgradePrice)
        {
            GetComponent<Button>().interactable = canUpgradeTower;

            var text = canUpgradeTower && upgradePrice.HasValue ? $"Upgrade ({upgradePrice.Value})" : "Upgrade";
            GetComponentInChildren<Text>().text = text;
        }
    }
}

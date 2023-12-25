using TDDemo.Assets.Scripts.Towers.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeImage : MonoBehaviour
    {
        public Image Image;

        public void UpdateImage(UpgradeNode upgrade)
        {
            if (upgrade != null)
            {
                Image.sprite = upgrade.Sprite;
            }
            else
            { 
                Image.sprite = null;
            }
        }
    }
}

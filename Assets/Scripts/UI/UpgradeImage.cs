using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeImage : MonoBehaviour
    {
        public void UpdateImage(UpgradeNode upgrade)
        {
            if (upgrade != null)
            {
                GetComponent<Image>().sprite = upgrade.Sprite;
            }
            else
            { 
                GetComponent<Image>().sprite = null;
            }
        }
    }
}

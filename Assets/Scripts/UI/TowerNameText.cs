using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerNameText : MonoBehaviour
    {
        public void UpdateText(TowerBehaviour tower)
        {
            if (tower != null)
            {
                GetComponent<Text>().text = tower.GetName();
            }
            else
            {
                GetComponent<Text>().text = string.Empty;
            }
        }
    }
}

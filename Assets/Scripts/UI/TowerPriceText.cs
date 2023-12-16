using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerPriceText : MonoBehaviour
    {
        public void UpdateText(TowerBehaviour tower)
        {
            if (tower != null)
            {
                GetComponent<Text>().text = $"Price: {tower.GetPrice()}";
            }
            else
            {
                GetComponent<Text>().text = string.Empty;
            }
        }
    }
}

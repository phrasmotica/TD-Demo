using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerPriceText : MonoBehaviour
    {
        public void UpdateText(TowerBehaviour tower)
        {
            GetComponent<Text>().text = $"Price: {tower.GetPrice()}";
        }
    }
}

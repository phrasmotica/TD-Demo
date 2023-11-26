using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerDescriptionText : MonoBehaviour
    {
        public void UpdateText(TowerBehaviour tower)
        {
            GetComponent<Text>().text = tower.Description;
        }
    }
}

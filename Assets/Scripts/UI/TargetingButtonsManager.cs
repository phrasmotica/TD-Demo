using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class TargetingButtonsManager : MonoBehaviour
    {
        public List<TargetingButton> Buttons;

        public void SetTower(TowerBehaviour tower)
        {
            foreach (var button in Buttons)
            {
                button.SetTower(tower);
            }
        }
    }
}

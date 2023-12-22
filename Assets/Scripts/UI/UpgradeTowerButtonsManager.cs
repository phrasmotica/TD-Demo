using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTowerButtonsManager : MonoBehaviour
    {
        public List<UpgradeTower> Buttons;

        public void SetTower(TowerBehaviour tower)
        {
            foreach (var button in Buttons)
            {
                button.SetTower(tower);
            }
        }

        public void Clear()
        {
            foreach (var button in Buttons)
            {
                button.Clear();
            }
        }
    }
}

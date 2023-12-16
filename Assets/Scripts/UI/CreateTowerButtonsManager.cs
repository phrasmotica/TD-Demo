using System.Collections.Generic;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class CreateTowerButtonsManager : MonoBehaviour
    {
        public List<CreateTower> Buttons;

        public void SetInteractable()
        {
            foreach (var button in Buttons)
            {
                button.SetInteractable();
            }
        }
    }
}

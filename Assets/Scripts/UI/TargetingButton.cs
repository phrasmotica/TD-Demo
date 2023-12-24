using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class TargetingButton : MonoBehaviour
    {
        public TowerController TowerController;

        public TargetMethod Method;

        public Button Button;

        public void SetTower(TowerBehaviour tower)
        {
            if (tower != null)
            {
                Button.interactable = tower.TargetMethod != Method;
            }
            else
            {
                Button.interactable = false;
            }
        }

        public void SetTargeting() => TowerController.SetTargetingSelectedTower(Method);
    }
}

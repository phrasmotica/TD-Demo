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

        public void SetInteractable(TowerBehaviour tower)
        {
            if (tower != null)
            {
                GetComponent<Button>().interactable = tower.TargetMethod != Method;
            }
        }

        // TODO: create an event for this
        public void SetTargeting() => TowerController.SetTargetingSelectedTower(Method);
    }
}

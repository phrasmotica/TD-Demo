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

        private void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(SetTargeting);

            TowerController.OnChangeSelectedTower += SetInteractable;
            TowerController.OnSetTargetMethodTower += SetInteractable;
        }

        private void SetInteractable(TowerBehaviour tower)
        {
            if (tower != null)
            {
                GetComponent<Button>().interactable = tower.TargetMethod != Method;
            }
        }

        private void SetTargeting() => TowerController.SetTargetingSelectedTower(Method);
    }
}

using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class SellTower : MonoBehaviour
    {
        public SpriteRenderer Sprite;

        public TowerController TowerController;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(TowerController.SellSelectedTower);

            TowerController.OnStartUpgradeSelectedTower += SetState;
            TowerController.OnFinishUpgradeSelectedTower += SetState;
            TowerController.OnChangeSelectedTower += SetState;
        }

        private void SetState(TowerBehaviour tower)
        {
            var canSellTower = tower != null;

            GetComponent<Button>().interactable = canSellTower;
            Sprite.color = canSellTower ? Color.white : Color.grey;
        }
    }
}

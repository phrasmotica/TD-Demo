using System;
using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerTooltip : MonoBehaviour
    {
        public event Action<TowerBehaviour> OnSetTower;

        private void Update()
        {
            transform.FollowMouse();
        }

        public void SetTower(TowerBehaviour tower) => OnSetTower?.Invoke(tower);
    }
}

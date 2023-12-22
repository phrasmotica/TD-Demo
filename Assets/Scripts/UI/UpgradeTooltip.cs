using TDDemo.Assets.Scripts.Extensions;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.UI
{
    public class UpgradeTooltip : MonoBehaviour
    {
        public UnityEvent<UpgradeNode> OnSetUpgrade;

        private void Update()
        {
            transform.FollowMouse();
        }

        public void SetUpgrade(UpgradeNode upgrade) => OnSetUpgrade.Invoke(upgrade);
    }
}

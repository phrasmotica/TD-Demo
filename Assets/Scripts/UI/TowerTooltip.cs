using TDDemo.Assets.Scripts.Extensions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class TowerTooltip : MonoBehaviour
    {
        private void Update()
        {
            transform.FollowMouse();
        }
    }
}

using TDDemo.Assets.Scripts.Enemies;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class TargetLine : MonoBehaviour
    {
        public LineRenderer LineRenderer;

        public TowerBehaviour SourceTower;

        public bool ShowLine { get; set; }

        public void Ready()
        {
            if (LineRenderer != null)
            {
                LineRenderer.SetPosition(0, transform.position);
            }
        }

        public void SetTarget(Enemy target)
        {
            var isTargeting = target != null && SourceTower.IsFiring();

            if (isTargeting && ShowLine)
            {
                LineRenderer.enabled = true;
                LineRenderer.SetPosition(1, target.transform.position);
            }
            else
            {
                LineRenderer.enabled = false;
            }
        }
    }
}

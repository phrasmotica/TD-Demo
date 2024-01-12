using System;
using TDDemo.Assets.Scripts.Enemies;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class TargetLine : MonoBehaviour
    {
        public LineRenderer LineRenderer;

        public TowerBehaviour SourceTower;

        public bool ShowLine { get; set; }

        private void Awake()
        {
            if (LineRenderer == null)
            {
                throw new InvalidOperationException($"TargetLine is missing its line renderer, object={gameObject.name}");
            }

            LineRenderer.enabled = false;
        }

        public void Ready()
        {
            LineRenderer.SetPosition(0, transform.position);
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

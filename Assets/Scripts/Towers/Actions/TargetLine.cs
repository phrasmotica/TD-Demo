using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class TargetLine : MonoBehaviour
    {
        public LineRenderer LineRenderer;

        public void Ready()
        {
            if (LineRenderer != null)
            {
                LineRenderer.SetPosition(0, transform.position);
            }
        }

        public void Show(Vector3 target)
        {
            LineRenderer.enabled = true;
            LineRenderer.SetPosition(1, target);
        }

        public void Hide()
        {
            LineRenderer.enabled = false;
        }
    }
}

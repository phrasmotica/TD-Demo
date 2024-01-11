using System.Collections;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class ShootLine : MonoBehaviour
    {
        public LineRenderer LineRenderer;

        [Range(0.1f, 5f)]
        public float LifetimeSeconds;

        public void Ready()
        {
            if (LineRenderer != null)
            {
                LineRenderer.SetPosition(0, transform.position);
            }
        }

        public void SetLine(Vector2 line)
        {
            LineRenderer.enabled = true;
            LineRenderer.SetPosition(1, transform.position + (Vector3) line);

            StartCoroutine(HideLine());
        }

        private IEnumerator HideLine()
        {
            yield return new WaitForSeconds(LifetimeSeconds);

            LineRenderer.enabled = false;
        }
    }
}

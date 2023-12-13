using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Distractions
{
    public class DistractionSource : BaseBehaviour
    {
        [Range(30, 60f)]
        public int Price;

        public bool IsPositioning { get; private set; }

        private void Start()
        {
            // would be nicer if we could handle this entirely within DistractionController,
            // but the mouse-up from the button's OnClick and the mouse-up detected by 
            // DistractionController.Update() occur in the same frame, so the distraction gets
            // placed immediately. Farming off the IsPositioning flag to this script seems to
            // get around the issue.
            IsPositioning = true;
        }

        public void Place()
        {
            IsPositioning = false;

            if (TryGetComponent<Animator>(out var animator))
            {
                animator.enabled = true;
            }
        }
    }
}

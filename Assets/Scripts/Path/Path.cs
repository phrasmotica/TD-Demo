using Assets.Scripts.UI;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Path
{
    public class Path : MonoBehaviour
    {
        /// <summary>
        /// The tower controller.
        /// </summary>
        public TowerController TowerController;

        /// <summary>
        /// Deselect tower when clicked.
        /// </summary>
        private void OnMouseUp()
        {
            using (var logger = new MethodLogger(nameof(Path)))
            {
                if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                {
                    if (TowerController.SelectedTower != null)
                    {
                        logger.Log("Deselecting tower");
                        TowerController.Deselect();
                    }
                    else
                    {
                        logger.Log("No tower to deselect!");
                    }
                }
            }
        }
    }
}

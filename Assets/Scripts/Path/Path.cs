using TDDemo.Assets.Scripts.UI;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace TDDemo.Assets.Scripts.Path
{
    public class Path : BaseBehaviour
    {
        /// <summary>
        /// The tower controller.
        /// </summary>
        public TowerController TowerController;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            logger = new MethodLogger(nameof(Path));
        }

        /// <summary>
        /// Deselect tower when clicked.
        /// </summary>
        private void OnMouseUp()
        {
            if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                var isOverGameObject = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
                if (!isOverGameObject)
                {
                    if (TowerController.TowerAlreadySelected)
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

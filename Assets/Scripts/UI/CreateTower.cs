using Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CreateTower : MonoBehaviour
    {
        /// <summary>
        /// The tower.
        /// </summary>
        public Tower Tower;

        /// <summary>
        /// Sets whether this button is interactable.
        /// </summary>
        public void SetInteractable(int money)
        {
            GetComponent<Button>().interactable = money >= Tower.Price;
        }
    }
}

using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerSelection : MonoBehaviour
    {
        public void SetSelected(bool isSelected)
        {
            GetComponent<SpriteRenderer>().enabled = isSelected;
        }
    }
}

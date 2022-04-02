using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class TowerSelection : MonoBehaviour
    {
        public TowerBehaviour TowerBehaviour;

        private void Awake()
        {
            TowerBehaviour.OnSelected += isSelected =>
            {
                GetComponent<SpriteRenderer>().enabled = isSelected;
            };
        }
    }
}

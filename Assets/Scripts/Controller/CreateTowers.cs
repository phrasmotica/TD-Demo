using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class CreateTowers : MonoBehaviour
    {
        /// <summary>
        /// The tower prefab.
        /// </summary>
        public GameObject TowerPrefab;

        /// <summary>
        /// The money store.
        /// </summary>
        private MoneyStore MoneyStore;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            MoneyStore = gameObject.GetComponent<MoneyStore>();
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void CreateTower()
        {
            using (var logger = new MethodLogger(nameof(CreateTowers)))
            {
                // only create if we can afford the tower
                if (MoneyStore.CanAfford(TowerPrefab.GetComponent<Tower>().Price))
                {
                    logger.Log("Creating tower");
                    var tower = Instantiate(TowerPrefab);
                    tower.GetComponent<Tower>().OnPlace = (price) => MoneyStore.AddMoney(-price);
                }
                else
                {
                    logger.Log("Cannot afford tower!");
                }
            }
        }
    }
}

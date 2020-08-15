using Assets.Scripts.Towers;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class TowersController : MonoBehaviour
    {
        /// <summary>
        /// The tower prefab.
        /// </summary>
        public GameObject TowerPrefab;

        /// <summary>
        /// The money controller.
        /// </summary>
        private MoneyController MoneyController;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            MoneyController = gameObject.GetComponent<MoneyController>();
        }

        /// <summary>
        /// Creates a tower.
        /// </summary>
        public void CreateTower()
        {
            using (var logger = new MethodLogger(nameof(TowersController)))
            {
                // only create if we can afford the tower
                if (MoneyController.CanAfford(TowerPrefab.GetComponent<Tower>().Price))
                {
                    logger.Log("Creating tower");
                    var tower = Instantiate(TowerPrefab);
                    tower.GetComponent<Tower>().OnPlace = (price) => MoneyController.AddMoney(-price);
                }
                else
                {
                    logger.Log("Cannot afford tower!");
                }
            }
        }
    }
}

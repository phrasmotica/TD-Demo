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
        /// Creates a tower.
        /// </summary>
        public void CreateTower()
        {
            using (var logger = new MethodLogger(nameof(CreateTowers)))
            {
                logger.Log("Creating tower");
                Instantiate(TowerPrefab);
            }
        }
    }
}

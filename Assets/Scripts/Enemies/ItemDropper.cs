using TDDemo.Assets.Scripts.Controller;
using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class ItemDropper : MonoBehaviour
    {
        public Enemy Enemy;

        public GameObject DropPrefab;

        [Range(0f, 1f)]
        public float DropChance;

        public PickupRouter PickupRouter { get; set; }

        private void Awake()
        {
            Enemy.OnPreKill += Enemy_OnPreKill;
        }

        private void Enemy_OnPreKill(Enemy enemy)
        {
            var r = new System.Random().NextDouble();
            if (r < DropChance)
            {
                Debug.Log($"Dropping a {DropPrefab.name}");

                var pos = enemy.transform.position + new Vector3(0, -1, 0);
                var drop = Instantiate(DropPrefab, pos, Quaternion.identity).GetComponent<DroppedItem>();
                drop.PickupRouter = PickupRouter;
            }
        }
    }
}

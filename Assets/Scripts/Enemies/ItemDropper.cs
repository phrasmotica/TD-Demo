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

        public BankManager Bank { get; set; }

        private void Awake()
        {
            Enemy.OnPreKill += Enemy_OnPreKill;
        }

        private void Enemy_OnPreKill(Enemy enemy)
        {
            // TODO: this generates 0.0 <= x < 1.0.
            // We need it to generate 0.0 < x <= 1.0
            var r = new System.Random().NextDouble();
            if (r < DropChance)
            {
                Debug.Log($"Dropping a {DropPrefab.name}");

                // TODO: change type argument to IItemDrop, which all drop
                // scripts should implement
                var pos = enemy.transform.position + new Vector3(0, -1, 0);
                var drop = Instantiate(DropPrefab, pos, Quaternion.identity).GetComponent<CouponDrop>();
                drop.Bank = Bank;
            }
        }
    }
}

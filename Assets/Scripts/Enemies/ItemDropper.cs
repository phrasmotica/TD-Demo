using System.Collections.Generic;
using TDDemo.Assets.Scripts.Controller;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class ItemDropper : MonoBehaviour
    {
        public List<DropSpecs> DropSpecs;

        public PickupRouter PickupRouter { get; set; }

        public void OnKill(Enemy enemy)
        {
            var random = new System.Random();

            var droppedCount = 0;

            for (var i = 0; i < DropSpecs.Count; i++)
            {
                var spec = DropSpecs[i];

                var r = random.NextDouble();
                if (r < spec.DropChance)
                {
                    // TODO: render each successive drop in a nicer position
                    var yPos = (float) -(1 + droppedCount * 0.5);
                    var pos = enemy.transform.position + new Vector3(0, yPos, 0);

                    Debug.Log($"Drop {i}: dropping a {spec.DropPrefab.name} at ({pos.x}, {pos.y})");

                    var drop = Instantiate(spec.DropPrefab, pos, Quaternion.identity).GetComponent<DroppedItem>();
                    drop.PickupRouter = PickupRouter;

                    droppedCount++;
                }
            }
        }
    }
}

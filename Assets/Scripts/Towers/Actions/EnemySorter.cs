using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public static class EnemySorter
    {
        public static List<GameObject> Sort(Transform source, IEnumerable<GameObject> enemies, TargetMethod method)
        {
            return (method switch
            {
                TargetMethod.NearestEnemy => enemies.OrderBy(source.GetDistanceToObject),
                TargetMethod.FurthestEnemy => enemies.OrderByDescending(source.GetDistanceToObject),
                TargetMethod.StrongestEnemy => enemies.OrderByDescending(o => o.GetComponent<Enemy>().HealthFraction),
                _ => enemies,
            }).ToList();
        }
    }
}

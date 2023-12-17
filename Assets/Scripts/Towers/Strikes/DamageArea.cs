using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Extensions;

namespace TDDemo.Assets.Scripts.Towers.Strikes
{
    public class DamageArea : IStrike
    {
        private readonly float _amount;

        private readonly float _radius;

        public TowerBehaviour SourceTower { get; set; }

        public DamageArea(float amount, float radius)
        {
            _amount = amount;
            _radius = radius;
        }

        public void Apply(Enemy enemy)
        {
            var enemies = GetNearbyEnemies(enemy);

            foreach (var e in enemies)
            {
                e.TakeDamage(_amount, false);
                e.LastDamagingTower = SourceTower;
            }
        }

        private List<Enemy> GetNearbyEnemies(Enemy enemy)
        {
            return SourceTower.GetEnemies()
                .Where(e => enemy.transform.GetDistanceToObject(e) <= _radius)
                .Select(e => e.GetComponent<Enemy>())
                .Where(e => e.CanBeTargeted())
                .ToList();
        }
    }
}

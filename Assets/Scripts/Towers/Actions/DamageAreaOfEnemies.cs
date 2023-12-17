using TDDemo.Assets.Scripts.Towers.Strikes;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class DamageAreaOfEnemies : StrikeProvider
    {
        [Range(1f, 5f)]
        public float Amount;

        [Range(1f, 3f)]
        public float Radius;

        public override IStrike CreateStrike() => new DamageArea(Amount, Radius)
        {
            SourceTower = SourceTower,
        };
    }
}

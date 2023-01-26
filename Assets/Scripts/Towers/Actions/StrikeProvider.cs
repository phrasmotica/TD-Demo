using TDDemo.Assets.Scripts.Towers.Strikes;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public abstract class StrikeProvider : MonoBehaviour
    {
        public TowerBehaviour SourceTower;

        public abstract IStrike CreateStrike();
    }
}

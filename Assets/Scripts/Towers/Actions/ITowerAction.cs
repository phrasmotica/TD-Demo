using System.Collections.Generic;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public interface ITowerAction
    {
        bool CanAct { get; set; }

        TargetMethod TargetMethod { get; set; }

        void Act(IEnumerable<GameObject> enemies);
    }
}

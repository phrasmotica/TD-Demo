using System.Collections.Generic;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public interface ITowerAction
    {
        bool ShowTargetLine { get; set; }

        bool CanAct { get; set; }

        TargetMethod TargetMethod { get; set; }

        void Ready();

        void Survey(IEnumerable<GameObject> enemies);

        void Act();
    }
}

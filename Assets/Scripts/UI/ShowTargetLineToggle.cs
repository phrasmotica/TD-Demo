using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class ShowTargetLineToggle : MonoBehaviour
    {
        public List<ITowerAction> Actions { get; set; }

        public void SetShowTargetLine(bool showTargetLine)
        {
            Actions.ForEach(a => a.ShowTargetLine = showTargetLine);
        }
    }
}

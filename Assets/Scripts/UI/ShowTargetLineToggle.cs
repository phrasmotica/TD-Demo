using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class ShowTargetLineToggle : MonoBehaviour
    {
        public TargetLine TargetLine { get; set; }

        public void SetShowTargetLine(bool showTargetLine)
        {
            TargetLine.ShowLine = showTargetLine;
        }
    }
}

using System.Collections.Generic;
using TDDemo.Assets.Scripts.Towers.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class ShowTargetLineToggle : MonoBehaviour
    {
        public List<ITowerAction> Actions { get; set; }

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(SetShowTargetLine);
        }

        private void SetShowTargetLine(bool showTargetLine)
        {
            Actions.ForEach(a => a.ShowTargetLine = showTargetLine);
        }
    }
}

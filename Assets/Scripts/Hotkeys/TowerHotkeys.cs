using System;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Hotkeys
{
    public class TowerHotkeys : MonoBehaviour
    {
        public Hotkey[] Hotkeys;

        private void Update()
        {
            foreach (var hotkey in Hotkeys)
            {
                if (Input.GetKeyUp(hotkey.KeyCode))
                {
                    hotkey.OnKey.Invoke();
                    return;
                }
            }
        }
    }

    [Serializable]
    public class Hotkey
    {
        public KeyCode KeyCode;

        public UnityEvent OnKey;
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace TDDemo.Assets.Scripts.UI
{
    public class KillCount : MonoBehaviour
    {
        public Text KillCountText;

        public void UpdateKillCount(int killCount)
        {
            KillCountText.text = $"{killCount}";
        }
    }
}

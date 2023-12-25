using TMPro;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class KillCount : MonoBehaviour
    {
        public TMP_Text KillCountText;

        public void UpdateKillCount(int killCount)
        {
            KillCountText.text = $"{killCount}";
        }
    }
}

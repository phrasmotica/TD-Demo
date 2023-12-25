using TDDemo.Assets.Scripts.Experience;
using TMPro;
using UnityEngine;

namespace TDDemo.Assets.Scripts.UI
{
    public class ExperienceBar : MonoBehaviour
    {
        public TMP_Text LevelText;

        public TMP_Text NextLevelText;

        public TMP_Text XpText;

        public LineRenderer LineBar;

        public LineRenderer LineShadow;

        public LineRenderer LineBackground;

        public float StartPosX { get; set; }

        public float MaxWidth { get; set; }

        private void Awake()
        {
            LineBar.positionCount = 2;
            LineBar.useWorldSpace = false;

            StartPosX = LineShadow.GetPosition(0).x;
            MaxWidth = LineShadow.GetPosition(1).x - StartPosX;
        }

        public void UpdateProgressBar(ExperienceContainer experience)
        {
            LevelText.text = $"{experience.Level}";
            NextLevelText.text = $"{experience.Level + 1}";

            XpText.text = $"{experience.CurrentXp}/{experience.NextLevelXp} XP";

            var width = MaxWidth * experience.GetProgressToNextLevel();

            LineBar.SetPosition(0, new Vector3(StartPosX, 0, 0));
            LineBar.SetPosition(1, new Vector3(StartPosX + width, 0, 0));
        }
    }
}

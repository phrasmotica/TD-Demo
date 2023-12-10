using System;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Music
{
    public class MusicController : MonoBehaviour
    {
        public AudioSource AudioSource;

        public AudioClip FirstStageMusic;
        public AudioClip SecondStageMusic;

        private void Awake()
        {
            StartMusic();
        }

        public void StartMusic()
        {
            AudioSource.clip = FirstStageMusic;
            AudioSource.loop = true;
            AudioSource.Play();
        }

        public void PauseMusic()
        {
            AudioSource.Pause();
        }

        public void ResumeMusic()
        {
            AudioSource.UnPause();
        }

        public void StopMusic()
        {
            AudioSource.Stop();
            AudioSource.time = 0;
        }

        public void SetMusicForStage(int stageNumber)
        {
            var newClip = ComputeNewClip(stageNumber);
            if (newClip != null)
            {
                AudioSource.Pause();
                var currentTime = AudioSource.time;

                AudioSource.clip = newClip;
                AudioSource.time = currentTime;

                AudioSource.Play();
            }
        }

        private AudioClip ComputeNewClip(int stageNumber) => (stageNumber % 2) switch
        {
            1 => FirstStageMusic,
            0 => SecondStageMusic,
            _ => throw new InvalidOperationException("This should never happen!"),
        };
    }
}

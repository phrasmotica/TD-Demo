using System;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Music
{
    public class MusicController : MonoBehaviour
    {
        public AudioClip FirstStageMusic;
        public AudioClip SecondStageMusic;

        private void Awake()
        {
            StartMusic();
        }

        public void StartMusic()
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = FirstStageMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void StopMusic()
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.Stop();
            audioSource.time = 0;
        }

        public void SetMusic(int stageNumber)
        {
            var newClip = ComputeNewClip(stageNumber);
            if (newClip != null)
            {
                var audioSource = GetComponent<AudioSource>();

                audioSource.Pause();
                var currentTime = audioSource.time;

                audioSource.clip = newClip;
                audioSource.time = currentTime;

                audioSource.Play();
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

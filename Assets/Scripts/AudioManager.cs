using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.CasualGames.SwoopGame
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource m_AudioSource;

        public AudioSource m_ObstacleAudioSource;

        public AudioSource m_ButtonsAudioSource;

        public AudioSource m_PlaneAudioSource;


        [SerializeField] private AudioClip m_GemAudio;

        [SerializeField] private AudioClip m_StarAudio;

        [SerializeField] private AudioClip m_CrashAudio;

        public static AudioManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void PlayMainmenuAudio()
        {
            if(m_AudioSource && !m_AudioSource.isPlaying) 
            {
                m_AudioSource.Play();
            }
        }

        public void PlayButtonAudio()
        {
            if (m_ButtonsAudioSource && !m_ButtonsAudioSource.isPlaying) 
            {
                m_ButtonsAudioSource.Play();
            }
        }
        public void PlayPlaneAudio()
        {
            if (m_PlaneAudioSource && !m_PlaneAudioSource.isPlaying)
            {
                m_PlaneAudioSource.Play();
            }
        }


        public void PlayObstaclesAudio(string Obstaclename)
        {
            if (m_ObstacleAudioSource)
            {
                if ((Obstaclename == "Score" || Obstaclename == "Fuel") && m_GemAudio != null)
                {
                    m_ObstacleAudioSource.PlayOneShot(m_GemAudio);
                }

                if (Obstaclename == "star" && m_StarAudio != null)
                {
                    m_ObstacleAudioSource.PlayOneShot(m_StarAudio);
                }

                if (Obstaclename == "Crash" && m_CrashAudio != null)
                {
                    m_ObstacleAudioSource.PlayOneShot(m_CrashAudio);
                }
            }

        }

    }
}

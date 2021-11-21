using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure {
    public class RandomAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        public class AudioBank
        {
            public string name;
            public AudioClip[] clips;
        }
        [SerializeField]
        private AudioBank m_AudioBank = new AudioBank();
        private AudioSource m_AudioSource;
        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomClip()
        {
            var clip = m_AudioBank.clips[Random.Range(0, m_AudioBank.clips.Length)];
            if (clip)
            {
                m_AudioSource.clip = clip;
                m_AudioSource.Play();
            }
        }
    }
}
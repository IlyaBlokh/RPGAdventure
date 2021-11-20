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
    }
}
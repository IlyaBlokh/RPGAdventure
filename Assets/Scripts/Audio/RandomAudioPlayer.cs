using UnityEngine;

namespace Audio {
    public class RandomAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        private class AudioBank
        {
            public string name;
            public AudioClip[] clips;
        }
        
        [SerializeField] private AudioBank m_AudioBank = new();
        private AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomClip()
        {
            AudioClip clip = m_AudioBank.clips[Random.Range(0, m_AudioBank.clips.Length)];
            if (clip)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
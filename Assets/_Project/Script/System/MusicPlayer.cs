using UnityEngine;

namespace System
{
    public class MusicPlayer : Singleton<MusicPlayer>
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void ChangeMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
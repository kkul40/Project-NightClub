using UnityEngine;

namespace System
{
    public class MusicSystem : Singleton<MusicSystem>
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        public void PlaySoundEffect(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }

        public void ChangeMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
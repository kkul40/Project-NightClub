using UnityEngine;

namespace System
{
    public class SFXPlayer : Singleton<SFXPlayer>
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlaySoundEffect(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
}
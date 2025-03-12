using UnityEngine;

namespace System.Music
{
    [Serializable]
    public class MusicPlayer
    {
        public AudioSource _audioSource;

        public BassDetector bassDedector;
        
        public MusicPlayer(AudioSource musicSource)
        {
            _audioSource = musicSource;
            bassDedector = new BassDetector(_audioSource);
            PauseMusic();
        }

        public void FrameUpdate(float deltaTime)
        {
            if (bassDedector != null)
                bassDedector.UpdateDetector();
        }

        public void SetMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
        }

        public void StartMusic()
        {
            _audioSource.Play();
        }

        public void PauseMusic()
        {
            _audioSource.Pause();
        }
    }
}
using Data;
using UnityEngine;
using UnityEngine.Audio;

namespace DiscoSystem.MusicPlayer
{
    public class MusicPlayer : Singleton<MusicPlayer>, ISavable
    {
        private AudioSource _audioSource;

        public BassDetector _bassDetector;
        
        [SerializeField] private AudioMixer mixer;
        public float MusicVolume { get; private set; }
        
        public void Initialize()
        {
            _audioSource = GetComponent<AudioSource>();
            _bassDetector = new BassDetector(_audioSource);
        }

        private void Update()
        {
            if (_bassDetector != null)
                _bassDetector.UpdateDetector();
        }

        public void ChangeMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void ResumeMusic()
        {
            _audioSource.Play();
        }

        public void PauseMusic()
        {
            _audioSource.Pause();
        }
        
        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        }

        public void LoadData(GameData gameData)
        {
            SetMusicVolume(gameData.gameSettingsData.MusicVolume);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.gameSettingsData.MusicVolume = MusicVolume;
        }
    }
}
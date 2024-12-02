using Data;
using UnityEngine;
using UnityEngine.Audio;

namespace System
{
    public class MusicPlayer : Singleton<MusicPlayer>, ISaveLoad
    {
        private AudioSource _audioSource;
        
        [SerializeField] private AudioMixer mixer;
        public float MusicVolume { get; private set; }
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            StopMusic();
        }

        public void ChangeMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
        
        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        }

        public void LoadData(GameData gameData)
        {
            SetMusicVolume(gameData.GameSettingsData.MusicVolume);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.GameSettingsData.MusicVolume = MusicVolume;
        }
    }
}
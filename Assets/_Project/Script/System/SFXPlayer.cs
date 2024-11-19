using Data;
using UnityEngine;
using UnityEngine.Audio;

namespace System
{
    public class SFXPlayer : Singleton<SFXPlayer>, ISaveLoad
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioMixer mixer;
        public float SoundVolume { get; private set; }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlaySoundEffect(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
        
        public void SetSoundVolume(float value)
        {
            SoundVolume = value;
            mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        }

        public void LoadData(GameData gameData)
        {
            SetSoundVolume(gameData.GameSettingsData.SoundVolume);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.GameSettingsData.SoundVolume = SoundVolume;
        }
    }
}
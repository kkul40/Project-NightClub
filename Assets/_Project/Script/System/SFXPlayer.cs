using Data;
using UnityEngine;
using UnityEngine.Audio;

namespace System
{
    public class SFXPlayer : Singleton<SFXPlayer>, ISaveLoad
    {
        private AudioSource _audioSource;

        private float timer = 0;
        [SerializeField] private AudioMixer mixer;

        [Header("Sound FX")]
        public AudioClip Succes;
        public AudioClip Error;
        
        public float SoundVolume { get; private set; }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlaySoundEffect(AudioClip audioClip, bool timerPlay = false)
        {
            float time = Time.time;

            if (timerPlay)
            {
                if (time - timer < audioClip.length)
                {
                    return;
                }
            }
            
            _audioSource.PlayOneShot(audioClip);
            timer = Time.time;
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
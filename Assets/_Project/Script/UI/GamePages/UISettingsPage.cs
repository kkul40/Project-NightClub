using System;
using Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.GamePages
{
    public class UISettingsPage : UIPageBase, ISaveLoad
    {
        public override PageType PageType { get; protected set; } = PageType.FullPage;
        
        private float _musicVolume = 1;
        private float _soundVolume = 1;

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;

        protected override void OnAwake()
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            soundVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
        }

        public void SetMusicVolume(float value)
        {
            _musicVolume = value;
            mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        }

        public void SetSoundVolume(float value)
        {
            _soundVolume = value;
            mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        }

        public void LoadData(GameData gameData)
        {
            SetMusicVolume(gameData.GameSettingsData.MusicVolume);
            SetSoundVolume(gameData.GameSettingsData.SoundVolume);
            musicVolumeSlider.value = _musicVolume;
            soundVolumeSlider.value = _soundVolume;
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.GameSettingsData.MusicVolume = _musicVolume;
            gameData.GameSettingsData.SoundVolume = _soundVolume;
        }
    }
}
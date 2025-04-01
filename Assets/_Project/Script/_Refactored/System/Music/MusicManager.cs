using System.Collections.Generic;
using Data;
using GameEvents;
using UnityEngine;
using UnityEngine.Audio;
using AudioSource = UnityEngine.AudioSource;

namespace System.Music
{
    public class MusicManager : SystemBase
    {
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundFXSource;
        [SerializeField] private AudioMixer mixer;

        [SerializeField] private SfxData _sfxData;
        private BassDetector _bassDetector;
        
        private float MasterVolume
        {
            get => GetVolume("MasterVolume");
            set => SetVolume("MasterVolume", value);
        }

        private float MusicVolume
        {
            get => GetVolume("MusicVolume");
            set => SetVolume("MusicVolume", value);
        }
        
        private float SfxVolume
        {
            get => GetVolume("SFXVolume");
            set => SetVolume("SFXVolume", value);
        }
        
        public void Initialize()
        {
            _bassDetector = new BassDetector(musicSource);
            
            GameEvent.Subscribe<Event_SetVolume>(SetVolume);
            GameEvent.Subscribe<Event_Sfx>(PlaySfx);
            GameEvent.Subscribe<Event_PlaySong>(PlaySong);
            GameEvent.Subscribe<Event_StopSong>(PauseSong);
        }
 
        private void Update()
        {
            _bassDetector.UpdateDetector();
        }

        private void SetVolume(Event_SetVolume eventSetVolume)
        {
            Debug.Log(eventSetVolume.Volume);
            switch (eventSetVolume.SourceVolume)
            {
                case GameEvents.SourceVolume.Master:
                    MasterVolume = eventSetVolume.Volume;
                    break;
                case GameEvents.SourceVolume.Music:
                    MusicVolume = eventSetVolume.Volume;
                    break;
                case GameEvents.SourceVolume.Sfx:
                    SfxVolume = eventSetVolume.Volume;
                    break;
            }
        }

        private void PlaySong(Event_PlaySong eventPlaySong)
        {
            ambientSource.Stop();
            
            musicSource.clip = eventPlaySong.Clip;
            musicSource.loop = false;
            musicSource.Play();
        }

        private void PauseSong(Event_StopSong eventStopSong)
        {
            musicSource.Stop();
            ambientSource.Play();
        }
        

        private float lastTime;
        private void PlaySfx(Event_Sfx eventSfx)
        {
            AudioClip clip = _sfxData.GetSfx(eventSfx.FXType);
            
            float time = Time.time;
            
            if (eventSfx.Delay)
            {
                if (time - lastTime < clip.length)
                {
                    return;
                }
            }
            
            soundFXSource.PlayOneShot(clip);
            lastTime = Time.time;
        }
      
        private void SetVolume(string name, float volume)
        {
            mixer.SetFloat(name, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }

        private float GetVolume(string name)
        {
            float volume;
            mixer.GetFloat(name, out volume);

            return volume;
        }
       

        public override void LoadData(GameData gameData)
        {
            throw new NotImplementedException();
        }

        public override void SaveData(ref GameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}
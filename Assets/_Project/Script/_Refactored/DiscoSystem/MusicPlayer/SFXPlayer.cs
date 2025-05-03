using System;
using Data;
using DiscoSystem.Building_System.GameEvents;
using UnityEngine;
using UnityEngine.Audio;

namespace DiscoSystem.MusicPlayer
{
    public enum SoundFXType
    {
        Click,
        Success,
        Error,
        UIClick,
        UIBack,
        NPCSelection,
        PropSelection,
        BuildingSuccess,
        BuildingError,
        MoneyAdd,
        MoneyRemove,
        CameraFocus,
    }
    // TODO Make This Class Part Of Music Player or just put all this into MusicPlayer
    public class SFXPlayer : Singleton<SFXPlayer>
    {
        private AudioSource _audioSource;

        private float timer = 0;
        [SerializeField] private AudioMixer mixer;

        [Header("Sound FX")] 
        public AudioClip Click;
        public AudioClip Succes;
        public AudioClip Error;
        public AudioClip UIClick;
        public AudioClip UIBack;
        public AudioClip NPCSelection;
        public AudioClip PropSelection;
        public AudioClip BuildingSuccess;
        public AudioClip BuildingError;
        public AudioClip MoneyAdd;
        public AudioClip MoneyRemove;
        public AudioClip CameraFocus;
        
        public float SoundVolume { get; private set; }

        public void Initialize()
        {
            _audioSource = GetComponent<AudioSource>();
            GameEvent.Subscribe<Event_Sfx>(PlaySoundFX);
        }

        private void PlaySoundFX(Event_Sfx sfxEvent)
        {
            switch (sfxEvent.FXType)
            {
                case SoundFXType.Click:
                    PlayAudioClip(Click, sfxEvent.Delay);
                    break;
                case SoundFXType.Success:
                    PlayAudioClip(Succes, sfxEvent.Delay);
                    break;
                case SoundFXType.Error:
                    PlayAudioClip(Error, sfxEvent.Delay);
                    break;
                case SoundFXType.UIClick:
                    PlayAudioClip(UIClick, sfxEvent.Delay);
                    break;
                case SoundFXType.UIBack:
                    PlayAudioClip(UIBack, sfxEvent.Delay);
                    break;
                case SoundFXType.NPCSelection:
                    PlayAudioClip(NPCSelection, sfxEvent.Delay);
                    break;
                case SoundFXType.PropSelection:
                    PlayAudioClip(PropSelection, sfxEvent.Delay);
                    break;
                case SoundFXType.BuildingSuccess:
                    PlayAudioClip(BuildingSuccess, sfxEvent.Delay);
                    break;
                case SoundFXType.BuildingError:
                    PlayAudioClip(BuildingError, sfxEvent.Delay);
                    break;
                case SoundFXType.MoneyAdd:
                    PlayAudioClip(MoneyAdd, sfxEvent.Delay);
                    break;
                case SoundFXType.MoneyRemove:
                    PlayAudioClip(MoneyRemove, sfxEvent.Delay);
                    break;
                case SoundFXType.CameraFocus:
                    PlayAudioClip(CameraFocus, sfxEvent.Delay);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sfxEvent.FXType), sfxEvent.Delay, null);
            }
        }

        private void PlayAudioClip(AudioClip clip, bool delay)
        {
            float time = Time.time;
            
            if (delay)
            {
                if (time - timer < clip.length)
                {
                    return;
                }
            }

            if (clip == null)
                Debug.LogError("Missing SFX File");
            else
                _audioSource.PlayOneShot(clip);
            
            timer = Time.time;
        }
        
        public void SetSoundVolume(float value)
        {
            SoundVolume = value;
            mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        }
    }
}
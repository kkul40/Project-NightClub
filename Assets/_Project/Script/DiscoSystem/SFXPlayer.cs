using System;
using Data;
using GameEvents;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace DiscoSystem
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
    public class SFXPlayer : Singleton<SFXPlayer>, ISavable
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

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            KEvent_SoundFX.OnSoundFXPlayed += PlaySoundFX;
        }

        private void OnDisable()
        {
            KEvent_SoundFX.OnSoundFXPlayed -= PlaySoundFX;
        }

        private void PlaySoundFX(SoundFXType fxType, bool delay)
        {
            switch (fxType)
            {
                case SoundFXType.Click:
                    PlayAudioClip(Click, delay);
                    break;
                case SoundFXType.Success:
                    PlayAudioClip(Succes, delay);
                    break;
                case SoundFXType.Error:
                    PlayAudioClip(Error, delay);
                    break;
                case SoundFXType.UIClick:
                    PlayAudioClip(UIClick, delay);
                    break;
                case SoundFXType.UIBack:
                    PlayAudioClip(UIBack, delay);
                    break;
                case SoundFXType.NPCSelection:
                    PlayAudioClip(NPCSelection, delay);
                    break;
                case SoundFXType.PropSelection:
                    PlayAudioClip(PropSelection, delay);
                    break;
                case SoundFXType.BuildingSuccess:
                    PlayAudioClip(BuildingSuccess, delay);
                    break;
                case SoundFXType.BuildingError:
                    PlayAudioClip(BuildingError, delay);
                    break;
                case SoundFXType.MoneyAdd:
                    PlayAudioClip(MoneyAdd, delay);
                    break;
                case SoundFXType.MoneyRemove:
                    PlayAudioClip(MoneyRemove, delay);
                    break;
                case SoundFXType.CameraFocus:
                    PlayAudioClip(CameraFocus, delay);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fxType), fxType, null);
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
            
            _audioSource.PlayOneShot(clip);
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
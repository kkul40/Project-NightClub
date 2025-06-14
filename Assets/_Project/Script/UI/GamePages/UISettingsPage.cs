using System.Collections.Generic;
using _Initializer;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character._Player;
using DiscoSystem.MusicPlayer;
using Framework.Context;
using Framework.Mvcs.View;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePages
{
    public class UISettingsPage : BaseView
    {
        public override PageType PageType { get; protected set; } = PageType.FullPage;

        [SerializeField] private List<GameObject> DropDownPages = new List<GameObject>();

        [SerializeField] private GameObject _lastDropDownPage;
        
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;

        public override void Initialize(IContext context)
        {
            base.Initialize(context);

            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            soundVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            
            ServiceLocator.Get<MusicPlayer>().SetMusicVolume(musicVolumeSlider.value);
            ServiceLocator.Get<SFXPlayer>().SetSoundVolume(soundVolumeSlider.value);
            
            musicVolumeSlider.onValueChanged.AddListener(ServiceLocator.Get<MusicPlayer>().SetMusicVolume);
            soundVolumeSlider.onValueChanged.AddListener(ServiceLocator.Get<SFXPlayer>().SetSoundVolume);
            
            GameEvent.Subscribe<Event_OnGameSave>(handle => SaveData());
        }

        public override void EventEnable()
        {
            musicVolumeSlider.value = ServiceLocator.Get<MusicPlayer>().MusicVolume;
            soundVolumeSlider.value = ServiceLocator.Get<SFXPlayer>().SoundVolume;

            OpenADropDownPage(_lastDropDownPage);
        }

        public override void EventDisable()
        {
        }

        public void OpenADropDownPage(GameObject pageObject)
        {
            CloseAllDropDown();
            pageObject.SetActive(true);
            _lastDropDownPage = pageObject;
        }

        private void CloseAllDropDown()
        {
            foreach (var page in DropDownPages)
            {
                page.SetActive(false);
            }
        }
        
        public void SaveData()
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", soundVolumeSlider.value);
        }
    }
}
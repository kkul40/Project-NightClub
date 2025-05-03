using System.Collections.Generic;
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
            
            MusicPlayer.Instance.SetMusicVolume(musicVolumeSlider.value);
            SFXPlayer.Instance.SetSoundVolume(soundVolumeSlider.value);
            
            musicVolumeSlider.onValueChanged.AddListener(MusicPlayer.Instance.SetMusicVolume);
            soundVolumeSlider.onValueChanged.AddListener(SFXPlayer.Instance.SetSoundVolume);
            
            GameEvent.Subscribe<Event_OnGameSave>(handle => SaveData());
        }

        public override void EventEnable()
        {
            musicVolumeSlider.value = MusicPlayer.Instance.MusicVolume;
            soundVolumeSlider.value = SFXPlayer.Instance.SoundVolume;

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
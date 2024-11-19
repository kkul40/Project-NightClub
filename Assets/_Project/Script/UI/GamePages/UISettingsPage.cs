using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePages
{
    public class UISettingsPage : UIPageBase
    {
        public override PageType PageType { get; protected set; } = PageType.FullPage;

        [SerializeField] private List<GameObject> DropDownPages = new List<GameObject>();

        [SerializeField] private GameObject _lastDropDownPage;
        
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;

        public static Action<bool> OnUISettingsToggle;

        protected override void OnAwake()
        {
            musicVolumeSlider.onValueChanged.AddListener(MusicPlayer.Instance.SetMusicVolume);
            soundVolumeSlider.onValueChanged.AddListener(SFXPlayer.Instance.SetSoundVolume);
        }

        private void OnEnable()
        {
            musicVolumeSlider.value = MusicPlayer.Instance.MusicVolume;
            soundVolumeSlider.value = SFXPlayer.Instance.SoundVolume;

            OpenADropDownPage(_lastDropDownPage);
            
            OnUISettingsToggle?.Invoke(true);
        }

        private void OnDisable()
        {
            OnUISettingsToggle?.Invoke(false);
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

        public void LoadSettingsData(GameData gameData)
        {
        }

        public void SaveSettingsData(ref GameData gameData)
        {
        }
    }
}
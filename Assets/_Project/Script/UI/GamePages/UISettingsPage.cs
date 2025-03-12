using System.Collections.Generic;
using Data;
using Framework.Context;
using Framework.Mvcs.View;
using GameEvents;
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
            
            GameEvent.Trigger(new Event_SetVolume(SourceVolume.Music, musicVolumeSlider.value));
            GameEvent.Trigger(new Event_SetVolume(SourceVolume.Sfx, soundVolumeSlider.value));
            
            musicVolumeSlider.onValueChanged.AddListener(value => GameEvent.Trigger(new Event_SetVolume(SourceVolume.Music, value)));
            soundVolumeSlider.onValueChanged.AddListener(value => GameEvent.Trigger(new Event_SetVolume(SourceVolume.Sfx, value)));
        }

        public override void EventEnable()
        {
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

        public void LoadSettingsData(GameData gameData)
        {
        }

        public void SaveSettingsData(ref GameData gameData)
        {
        }
    }
}
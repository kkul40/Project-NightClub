using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using Framework.Context;
using Framework.Mvcs.View;
using PropBehaviours;
using UI.PopUp;
using UI.StartMenu.Popup;
using UnityEngine;

namespace UI.GamePages
{
    public class UIPageManager : Singleton<UIPageManager>, ISavable
    {
        [SerializeField] private UISettingsPage _settingsPage;
        private HashSet<BaseView> _uiPageBases = new HashSet<BaseView>();

        public Canvas GetCanvas
        {
            get
            {
                if (TryGetComponent(out Canvas canvas)) return canvas;

                return null;
            }
        }

        public void Initialize()
        {
            _uiPageBases = FindObjectsByType<BaseView>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToHashSet();
            
            foreach (var page in _uiPageBases)
            {
                if(!page.IsInitialized)
                    page.Initialize(new Context());
                
                page.EventEnable();
            }

            CloseAllPages();
            
            GameEvent.Subscribe<Event_SelectionReset>( handle => CloseAllPages(PageType.MiniPage));
            GameEvent.Trigger(new Event_ResetCursorSelection());
        }

        private void Update()
        {
            if (InputSystem.Instance.GetEscape(InputType.WasPressedThisFrame))
            {
                if (!IsAnyUIToggled())
                {
                    UISettingsPage page = GetPage(typeof(UISettingsPage)) as UISettingsPage;
                    page.ToggleView(true);
                    
                }
                else
                {
                    CloseAllPages(PageType.FullPage);
                    CloseAllPages(PageType.MiniPage);
                }
            }
            
            // if (InputSystem.Instance.CancelClick || InputSystem.Instance.LeftClickOnWorld)
            // {
            //     CloseAllPages(PageType.MiniPage);
            // }
        }

        public void ShowDrinkPage(Bar bar)
        {
            UIPickADrinkPage page = GetPage(typeof(UIPickADrinkPage)) as UIPickADrinkPage;
            page.Show(bar);
        }

        public void ShowActionSelectionPage(object data)
        {
            UIActionSelectionPage page = GetPage(typeof(UIActionSelectionPage)) as UIActionSelectionPage;
            page.Show(data);
        }

        public void ShowPropInfo(IPropUnit unit)
        {
            UIPropInfo page = GetPage(typeof(UIPropInfo)) as UIPropInfo;
            page.Show(unit);
        }

        public void ShowLoadPage()
        {
            UILoadPage page = GetPage(typeof(UILoadPage)) as UILoadPage;
            page.ToggleView(true);
        }

        public void ShowTextFieldPopup(string message, Action<string> callback)
        {
            TextFieldPopup page = GetPage(typeof(TextFieldPopup)) as TextFieldPopup;
            page.Show(message, callback);
        }

        public void ShowAreYouSurePopup(string message, Action callback)
        {
            UIAreYouSurePopup page = GetPage(typeof(UIAreYouSurePopup)) as UIAreYouSurePopup;
            page.Show(message, callback);
        }

        // public void RequestAPage<T>(Type requestedPage, T data = null) where T : class
        // {
        //     var page = GetPage(requestedPage);
        //
        //     CloseAllPages(PageType.MiniPage);
        //
        //     if (data != null)
        //     {
        //         page.Show(data);
        //         return;
        //     }
        //
        //     page.Show();
        // }

        public bool IsPageToggled(Type reqeustedPage)
        {
            var page = GetPage(reqeustedPage);
            return page.IsToggled;
        }
        
        /// <summary>
        /// For Button Calls
        /// </summary>
        /// <param name="requestedPage"></param>
        public void ToggleAPage(BaseView requestedPage)
        {
            var type = requestedPage.GetType();
            var page = GetPage(type);
            page.ToggleView();

            if (page.IsToggled)
            {
                if (page.PageType == PageType.FullPage)
                {
                    CloseAllPages(PageType.MiniPage);
                }
            }
        }

        public void CloseAPage(Type requestedPage)
        {
            var page = GetPage(requestedPage);
            
            page.ToggleView(false);
        }

        public void CloseAllPages()
        {
            foreach (var page in _uiPageBases)
            {
                page.ToggleView(false);
            }
        }

        public void CloseAllPages(PageType pageType)
        {
            foreach (var page in _uiPageBases)
            {
                if(page.PageType == pageType)
                    page.ToggleView(false);
            }
        }

        private BaseView GetPage(Type findPage)
        {
            var page = _uiPageBases.FirstOrDefault(page => page.GetType() == findPage);
            if (page == null)
            {
                Debug.LogError("Page Could Not Found : " + findPage);
            }
            return page;
        }

        public bool IsAnyUIToggled()
        {
            foreach (var pages in _uiPageBases)
                if (pages.IsToggled)
                    return true;

            return false;
        }

        public void LoadData(GameData gameData)
        {
            _settingsPage.LoadSettingsData(gameData);
        }

        public void SaveData(ref GameData gameData)
        {
            _settingsPage.SaveSettingsData(ref gameData);
        }
        
        private void OnDestroy()
        {
            foreach (var page in _uiPageBases)
            {
                page.EventDisable();
                page.Dispose();
            }
        }
    }
}
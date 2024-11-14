using System;
using System.Collections.Generic;
using System.Linq;
using PropBehaviours;
using UnityEngine;

namespace UI.GamePages
{
    
    public class UIPageManager : Singleton<UIPageManager>
    {
        private HashSet<UIPageBase> _uiPageBases = new HashSet<UIPageBase>();

        private void Awake()
        {
            _uiPageBases = FindObjectsByType<UIPageBase>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToHashSet();
        }

        private void Start()
        {
            CloseAllPages();
        }

        private void Update()
        {
            if (InputSystem.Instance.Esc)
            {
                CloseAllPages(PageType.FullPage);
            }

            if (InputSystem.Instance.CancelClick || InputSystem.Instance.LeftClickOnWorld)
            {
                CloseAllPages(PageType.MiniPage);
            }
        }

        public void RequestAPage<T>(Type requestedPage, T data = null) where T : class
        {
            var page = GetPage(requestedPage);

            CloseAllPages(PageType.MiniPage);

            if (data != null)
            {
                page.Show(data);
                return;
            }

            page.Show();
        }

        public bool IsPageToggled(Type reqeustedPage)
        {
            var page = GetPage(reqeustedPage);
            return page.isToggled;
        }
        
        /// <summary>
        /// For Button Calls
        /// </summary>
        /// <param name="requestedPage"></param>
        public void ToggleAPage(UIPageBase requestedPage)
        {
            var type = requestedPage.GetType();
            var page = GetPage(type);
            page.Toggle();

            if (page.isToggled)
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
            
            page.Hide();
        }

        public void CloseAllPages()
        {
            foreach (var page in _uiPageBases)
            {
                page.Hide();
            }
        }

        public void CloseAllPages(PageType pageType)
        {
            foreach (var page in _uiPageBases)
            {
                if(page.PageType == pageType)
                    page.Hide();
            }
        }

        private UIPageBase GetPage(Type findPage)
        {
            var page = _uiPageBases.FirstOrDefault(page => page.GetType() == findPage);
            if (page == null)
            {
                Debug.LogError("Page Could Not Found : " + findPage.ToString());
            }
            return page;
        }
    }
}
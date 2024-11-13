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

            if (InputSystem.Instance.LeftClickOnWorld)
            {
                var hitTransform = InputSystem.Instance.GetHitTransform();

                if (hitTransform.TryGetComponent(out IInteractable interactable))
                {
                    if(interactable.Interaction == eInteraction.None)
                        CloseAllPages(PageType.MiniPage);
                }
                else
                    CloseAllPages(PageType.MiniPage);
            }
        }

        public void RequestAPage<T>(UIPageBase requestedPage, T data = null) where T : class
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

        public bool IsPageIsToggled(UIPageBase reqeustedPage)
        {
            var page = GetPage(reqeustedPage);
            
            if (page.PageType == PageType.FullPage)
            {
                CloseAllPages(PageType.MiniPage);
            }
            
            return page.isToggled;
        }
        
        /// <summary>
        /// For Button Call
        /// </summary>
        /// <param name="requestedPage"></param>
        public void ToggleAPage(UIPageBase requestedPage)
        {
            var page = GetPage(requestedPage);
            page.Toggle();

            if (page.isToggled)
            {
                if (page.PageType == PageType.FullPage)
                {
                    CloseAllPages(PageType.MiniPage);
                }
            }
        }

        public void CloseAPage<T>(T requestedPage) where T : UIPageBase
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

        private UIPageBase GetPage(UIPageBase findPage)
        {
            var page = _uiPageBases.FirstOrDefault(page => page.GetType() == findPage.GetType());
            if (page == null)
            {
                Debug.LogError("Page Could Not Found : " + findPage.ToString());
            }
            return page;
        }
    }
}
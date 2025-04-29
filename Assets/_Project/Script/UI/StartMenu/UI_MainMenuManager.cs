using System;
using System.Collections.Generic;
using DiscoSystem;
using UnityEngine;

namespace UI.StartMenu
{
    public class UI_MainMenuManager : MonoBehaviour
    {
        public static UI_MainMenuManager Instance;
        public List<Menu_UI_Page> pages;

        private readonly Stack<Menu_UI_Page> _stack = new();
        private Menu_UI_Page currentPage;

        private void Awake()
        {
            Instance = this;

            foreach (var page in pages)
            {
                if (!page.gameObject.activeInHierarchy)
                {
                    page.gameObject.SetActive(true);
                }
                page.Initialize();
                page.HideImmidiatly();
            }

            currentPage = pages[0];
            _stack.Push(currentPage);

            currentPage.ShowImmidiatly();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButton();
            }
        }

        public void OpenNewPage(Menu_UI_Page page)
        {
            var foundPage = FindPage(page);

            if (foundPage != null)
            {
                currentPage.Hide(Menu_UI_Page.ePushAnimation.PushLeft);
                currentPage = foundPage;
                _stack.Push(currentPage);
                currentPage.Show(Menu_UI_Page.ePushAnimation.PushRight);
            }
            else
            {
                Debug.LogError("Page not Found");
            }
        }

        public void OpenNewPageImmidiatly(Menu_UI_Page page)
        {
            var foundPage = FindPage(page);
            if (foundPage != null)
            {
                currentPage.HideImmidiatly();
                currentPage = foundPage;
                _stack.Push(currentPage);
                currentPage.ShowImmidiatly();
            }

        }

        private Menu_UI_Page FindPage(Menu_UI_Page page)
        {
            foreach (var p in pages)
                if (p == page)
                    return p;
            return null;
        }

        public void OnBackButton()
        {
            if (_stack.Count == 1) return;

            var pop = _stack.Pop();
            pop.Hide(Menu_UI_Page.ePushAnimation.PushRight);
            var peek = _stack.Peek();
            peek.Show(Menu_UI_Page.ePushAnimation.PushLeft);
            currentPage = peek;
        }
    }
}
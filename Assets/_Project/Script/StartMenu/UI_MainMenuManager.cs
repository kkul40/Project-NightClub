using System.Collections.Generic;
using UnityEngine;

namespace StartMenu
{
    public class UI_MainMenuManager : MonoBehaviour
    {
        public static UI_MainMenuManager Instance;
        public List<UI_Page> pages;
        private UI_Page currentPage;

        private Stack<UI_Page> _stack = new Stack<UI_Page>();


        private void Awake()
        {
            Instance = this;

            foreach (var page in pages)
            {
                page.Initiliase();
            }
            currentPage = pages[0];
            _stack.Push(currentPage);
            
            currentPage.ShowImmidiatly();
        }

        public void OpenNewPage(UI_Page page)
        {
            UI_Page foundPage = FindPage(page);

            if (foundPage != null)
            {
                currentPage.Hide(UI_Page.ePushAnimation.PushLeft);
                currentPage = foundPage;
                _stack.Push(currentPage);
                currentPage.Show(UI_Page.ePushAnimation.PushRight);
            }
            else
            {
                Debug.LogError("Page not Found");
            }
        }

        private UI_Page FindPage(UI_Page page)
        {
            foreach (var p in pages)
            {
                if (p == page)
                {
                    return p;
                }
            }
            return null;
        }

        public void OnBackButton()
        {
            if (_stack.Count == 1) return;
            
            UI_Page pop = _stack.Pop();
            pop.Hide(UI_Page.ePushAnimation.PushRight);
            UI_Page peek = _stack.Peek();
            peek.Show(UI_Page.ePushAnimation.PushLeft);
            currentPage = peek;
            Debug.Log("Get Back");
        }
    }
}
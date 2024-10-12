using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIPageManager : Singleton<UIPageManager>
    {
        private List<UIPageBase> _uiPageBases = new();

        private Stack<UIPageBase> _uiPageQue = new Stack<UIPageBase>();

        // private UIPageBase lastToggle;

        private void Start()
        {
            CloseAll();
        }

        private void Update()
        {
            if (InputSystem.Instance.Esc && _uiPageQue.Count > 0)
            {
                var page = _uiPageQue.Pop();
                page.Hide();
            }
        }

        private void CloseAll(bool dontToggleLast = false)
        {
            while (_uiPageQue.Count > 0)
            {
                var page = _uiPageQue.Pop();
                page.Hide();
            }
        }

        public void RegisterOnShow(UIPageBase uiPageBase)
        {
            _uiPageQue.Push(uiPageBase);
            // lastToggle = uiPageBase;
        }

        public void RegisterOnHide(UIPageBase uiPageBase)
        {
            Stack<UIPageBase> tempStack = new Stack<UIPageBase>();

            while (_uiPageQue.Count > 0)
            {
                UIPageBase topItem = _uiPageQue.Pop();
                if (!EqualityComparer<UIPageBase>.Default.Equals(topItem, uiPageBase))
                {
                    tempStack.Push(topItem);
                }
            }

            while (tempStack.Count > 0)
            {
                _uiPageQue.Push(tempStack.Pop());
            }
        }

        public void RegisterForPage(UIPageBase uiPageBase)
        {
            _uiPageBases.Add(uiPageBase);
        }
    }
}
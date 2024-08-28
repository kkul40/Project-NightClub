using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIMenuManager : Singleton<UIMenuManager>
    {
        private List<UIMenuBase> _uiPageBases = new();

        private UIMenuBase _lastMenu;

        private void Awake()
        {
        }

        private void Start()
        {
            _uiPageBases = FindObjectsOfType<MonoBehaviour>().OfType<UIMenuBase>().ToList();
            CloseAllUIPages();
        }

        private void Update()
        {
            if (InputSystem.Instance.Esc)
            {
                if (_lastMenu != null)
                {
                    _lastMenu.Toggle(false);
                }
            }
        }

        public void HandleNewPageOpening(UIMenuBase source)
        {
            foreach (var page in _uiPageBases)
            {
                if (page == source) continue;
                page.Hide();
            }

            _lastMenu = source;
        }

        public void CloseAllUIPages()
        {
            foreach (var page in _uiPageBases)
                page.Hide();

            _lastMenu = null;
        }
    }
}
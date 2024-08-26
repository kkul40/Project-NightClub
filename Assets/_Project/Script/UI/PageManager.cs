using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class PageManager : Singleton<PageManager>
    {
        private List<UIPageBase> _uiPageBases = new();

        private UIPageBase _lastPage;

        private void Awake()
        {
            _uiPageBases = FindObjectsOfType<MonoBehaviour>().OfType<UIPageBase>().ToList();
            HandleNewUIPageToggle(null);
        }

        private void Update()
        {
            if (InputSystem.Instance.Esc)
            {
                if (_lastPage != null)
                {
                    _lastPage.Toggle(false);
                }
            }
        }

        public void HandleNewUIPageToggle(UIPageBase source)
        {
            _lastPage = source;
            foreach (var page in _uiPageBases) page.Toggle(false);
        }
    }
}
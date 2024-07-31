using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class PageManager : Singleton<PageManager>
    {
        private List<UIPageBase> _uiPageBases = new();

        private void Awake()
        {
            _uiPageBases = FindObjectsOfType<MonoBehaviour>().OfType<UIPageBase>().ToList();
            HandleNewUIPageToggle(null);
        }

        public void HandleNewUIPageToggle(UIPageBase source)
        {
            foreach (var page in _uiPageBases) page.Toggle(false);
        }
    }
}
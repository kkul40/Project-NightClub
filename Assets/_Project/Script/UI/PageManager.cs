using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class PageManager : Singleton<PageManager>
    {
        private List<UIPageBase> _uiPageBases = new List<UIPageBase>();

        private void Awake()
        {
            _uiPageBases = FindObjectsOfType<MonoBehaviour>().OfType<UIPageBase>().ToList();
            HandleNewUIPageToggle(null);
        }

        public void HandleNewUIPageToggle(UIPageBase source)
        {
            // source if you wanna do something with it
            foreach (var page in _uiPageBases)
            {
                page.Toggle(false);
            }
        }
    }
}
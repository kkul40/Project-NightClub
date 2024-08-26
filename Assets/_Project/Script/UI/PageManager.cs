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
            HandleNewUIPageToggle(null, false);
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

        public void HandleNewUIPageToggle(UIPageBase source, bool toggle)
        {
            _lastPage = source;
            
            foreach (var page in _uiPageBases)
            {
                if (page == source)
                {
                    if(toggle)
                        page.Show();
                    else
                        page.Hide();
                    
                    continue;
                }
                page.Hide();
            }
        }
    }
}
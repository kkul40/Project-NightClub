using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIPageManager : Singleton<UIPageManager>
    {
        private List<UIPageBase> _uiPageBases = new();

        private UIPageBase lastToggle;

        private void Awake()
        {
            _uiPageBases = FindObjectsOfType<MonoBehaviour>().OfType<UIPageBase>().ToList();
            CloseAll();
        }

        private void Update()
        {
            if (InputSystem.Instance.Esc && lastToggle != null)
            {
                lastToggle.Toggle(false);   
            }
        }

        public void ToggleNewPage(UIPageBase source)
        {
            source.Toggle();
            lastToggle = source;
            CloseAll(true);
        }

        private void CloseAll(bool dontToggleLast = false)
        {
            foreach (var page in _uiPageBases)
            {
                if (dontToggleLast)
                {
                    if(page == lastToggle) continue;
                }
                
                page.Toggle(false);
            }
        }
    }
}
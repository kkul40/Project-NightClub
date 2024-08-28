using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIWorldPopUpManager : Singleton<UIWorldPopUpManager>
    {
        private List<UIPopUpBase> _uiPagePopUps = new();

        private UIPopUpBase lastPopUp;

        private void Awake()
        {
            _uiPagePopUps = FindObjectsOfType<MonoBehaviour>().OfType<UIPopUpBase>().ToList();
        }

        private void Update()
        {
            if (lastPopUp != null)
            {
                lastPopUp.UpdatePosition();
            }

            if (InputSystem.Instance.Esc)
            {
                if (lastPopUp != null)
                {
                    lastPopUp.Hide();
                }
            }
        }

        public void HandleNewWorldPopUpPage<T>(Vector3 worldPosition, bool toggle) where T : UIPopUpBase
        {
            CloseAll();

            foreach (var page in _uiPagePopUps)
            {
                if (page is T type)
                {
                    
                    if (toggle)
                    {
                        type.OnShow(worldPosition);
                        type.Show();
                    }
                    else
                        type.Hide();

                    
                    lastPopUp = type;
                    return;
                }
            }
        }

        public void CloseAll()
        {
            foreach (var page in _uiPagePopUps)
            {
                page.Hide();
            }

            lastPopUp = null;
        }
    }
}
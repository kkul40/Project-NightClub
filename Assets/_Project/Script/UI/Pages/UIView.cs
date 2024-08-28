using System;
using UnityEngine;

namespace UI
{
    public class UIView : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            IsActive = true;
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }
    }
}
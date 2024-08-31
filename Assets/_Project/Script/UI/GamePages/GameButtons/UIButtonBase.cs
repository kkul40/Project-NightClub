using System;
using PropBehaviours;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public abstract class UIButtonBase : MonoBehaviour, IButton
    {
        public Button button;
        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        protected virtual void Start()
        {
        }

        public virtual void OnHover()
        {
        }

        public virtual void OnClick()
        {
            // EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public interface IButton
    {
        void OnHover();
        void OnClick();
    }
}
using UnityEngine;

namespace UI.GamePages
{
    public enum PageType
    {
        FullPage,
        MiniPage,
    }
    
    public abstract class UIPageBase : MonoBehaviour
    {
        public bool isToggled = false;

        public abstract PageType PageType { get; protected set; }

        protected virtual void OnAwake(){}
        protected virtual void OnShow(){}
        protected virtual void OnShow<T>(T data){}
        protected virtual void OnHide(){}

        private void Awake()
        {
            OnAwake();
            Show();
        }

        public void Toggle()
        {
            if (isToggled)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            isToggled = true;
            gameObject.SetActive(true);
            OnShow();
        }

        public void Show<T>(T data)
        {
            isToggled = true;
            gameObject.SetActive(true);
            OnShow(data);
        }

        public void Hide()
        {
            isToggled = false;
            gameObject.SetActive(false);
            OnHide();
        }
    }
}
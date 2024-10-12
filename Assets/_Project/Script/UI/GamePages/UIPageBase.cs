using System;

namespace UI
{
    public class UIPageBase : Singleton<UIPageBase>
    {
        public bool isToggled = false;

        protected virtual void OnAwake(){}
        protected virtual void OnShow(){}
        protected virtual void OnHide(){}

        private void Awake()
        {
            UIPageManager.Instance.RegisterForPage(this);
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
            UIPageManager.Instance.RegisterOnShow(this);
            OnShow();
        }

        public void Hide()
        {
            isToggled = false;
            gameObject.SetActive(false);
            UIPageManager.Instance.RegisterOnHide(this);
            OnHide();
        }
    }
}
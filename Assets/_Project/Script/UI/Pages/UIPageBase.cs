using System;

namespace UI
{
    public class UIPageBase : Singleton<UIPageBase>
    {
        public virtual void Toggle(bool toggle)
        {
            PageManager.Instance.HandleNewUIPageToggle(this, toggle);
        }

        public virtual void Toggle()
        {
            PageManager.Instance.HandleNewUIPageToggle(this, gameObject.activeInHierarchy ? false : true);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
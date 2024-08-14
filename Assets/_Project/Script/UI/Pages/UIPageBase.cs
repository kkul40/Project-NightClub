using System;

namespace UI
{
    public class UIPageBase : Singleton<UIPageBase>
    {
        public virtual void Toggle(bool toggle)
        {
            if (toggle)
            {
                PageManager.Instance.HandleNewUIPageToggle(this);
                gameObject.SetActive(true);
                return;
            }

            gameObject.SetActive(false);
        }

        public virtual void Toggle()
        {
            if (isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                return;
            }

            PageManager.Instance.HandleNewUIPageToggle(this);
            gameObject.SetActive(true);
        }
    }
}
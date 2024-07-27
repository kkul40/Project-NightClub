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
                this.gameObject.SetActive(true);
                return;
            }
            this.gameObject.SetActive(false);
        }
        
        public virtual void Toggle()
        {
            if (isActiveAndEnabled)
            {
                this.gameObject.SetActive(false);
                return;
            }
            PageManager.Instance.HandleNewUIPageToggle(this);
            this.gameObject.SetActive(true);
        }
    }
}
using System;

namespace UI
{
    public class UIPageBase : Singleton<UIPageBase>
    {
        public virtual void Toggle(bool? toggle = null)
        {
            if (toggle != null)
            {
                gameObject.SetActive(toggle ?? false);
                return;
            }
            
            if (isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
        }
    }
}
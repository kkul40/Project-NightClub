using System;

namespace UI
{
    public class UIMenuBase : UIView
    {
        public virtual void Toggle(bool toggle)
        {
            if (toggle)
            {
                Show();
            }
            else
            {
                Hide();
            }
            
            UIMenuManager.Instance.HandleNewPageOpening(this);
        }

        public virtual void Toggle()
        {
            if (IsActive)
            {
                Hide();
            }
            else
                Show();

            UIMenuManager.Instance.HandleNewPageOpening(this);
        }
    }
}
using System;
using UnityEngine;

namespace UI
{
    public class UIButton : MonoBehaviour, IButton
    {
        public virtual void OnHover()
        {
        }

        public virtual void OnClick()
        {
        }
    }

    public interface IButton
    {
        void OnHover();
        void OnClick();
    }
}
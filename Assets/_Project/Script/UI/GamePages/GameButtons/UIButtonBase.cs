using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePages.GameButtons
{
    public abstract class UIButtonBase : MonoBehaviour, IButton
    {
        public Button button;
        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public virtual void OnAwake()
        {
            
        }

        protected virtual void Start()
        {
        }

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
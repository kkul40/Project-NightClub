using DG.Tweening;
using UnityEngine;

namespace StartMenu
{
    public abstract class UI_Page : MonoBehaviour
    {
        public enum ePushAnimation
        {
            PushRight,
            PushLeft,
        }

        private RectTransform _rectTransform;

        public bool isOpen = false;
        private float treshHold = 0.5f;
        private float screenWeight = 1920;
        private float speed = 7;

        private float animDuration = 1;
    
        [SerializeField] protected CanvasGroup _canvasGroup;

 
        public void Initiliase()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public virtual void Show(ePushAnimation ePushAnimation)
        {
            switch (ePushAnimation)
            {
                case ePushAnimation.PushRight:
                    _rectTransform.anchoredPosition = new Vector2(screenWeight, 0);
                    break;
                case ePushAnimation.PushLeft:
                    _rectTransform.anchoredPosition = new Vector2(-screenWeight, 0);
                    break;
            }
        
            _canvasGroup.alpha = 0;
            _rectTransform.DOAnchorPos(Vector2.zero, animDuration).OnComplete(SetShow);
            _canvasGroup.DOFade(1, animDuration);
        }

        public virtual void ShowImmidiatly()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            isOpen = true;
        
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    
        public virtual void Hide(ePushAnimation ePushAnimation)
        {
            switch (ePushAnimation)
            {
                case ePushAnimation.PushRight:
                    _rectTransform.DOAnchorPos(new Vector2(screenWeight, 0), animDuration);
                    break;
                case ePushAnimation.PushLeft:
                    _rectTransform.DOAnchorPos(new Vector2(-screenWeight, 0), animDuration);
                    break;
            }
            _canvasGroup.interactable = false;
            isOpen = false;
            _canvasGroup.DOFade(0, animDuration / 5);
        }


        private void SetShow()
        {
            _canvasGroup.alpha = 1;
            _rectTransform.anchoredPosition = Vector2.zero;
            _canvasGroup.interactable = true;
            isOpen = true;
        }

        private void SetHide()
        {
            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = Vector2.zero;
            _canvasGroup.interactable = false;
            isOpen = false;
        }
    }
}
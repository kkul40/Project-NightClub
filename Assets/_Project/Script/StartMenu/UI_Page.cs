using DG.Tweening;
using UnityEngine;

namespace StartMenu
{
    public abstract class UI_Page : MonoBehaviour
    {
        public enum ePushAnimation
        {
            PushRight,
            PushLeft
        }

        public bool isOpen;

        [SerializeField] protected CanvasGroup _canvasGroup;

        private readonly float animDuration = 1;
        private readonly float screenWeight = 1920;

        private RectTransform _rectTransform;
        private float speed = 7;
        private float treshHold = 0.5f;


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
            _canvasGroup.interactable = true;
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

            isOpen = false;
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(0, animDuration / 5);
        }

        public virtual void HideImmidiatly()
        {
            isOpen = false;
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
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
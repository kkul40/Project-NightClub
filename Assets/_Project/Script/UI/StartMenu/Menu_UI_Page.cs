using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

namespace UI.StartMenu
{
    public abstract class Menu_UI_Page : MonoBehaviour
    {
        public enum ePushAnimation
        {
            PushRight,
            PushLeft
        }

        public bool isOpen;

        [SerializeField] protected CanvasGroup _canvasGroup;

        protected readonly float animDuration = 1;
        private readonly float screenWeight = 1920;

        private RectTransform _rectTransform;
        private float speed = 7;
        private float treshHold = 0.5f;

        private Tween _canvasTween;

        public virtual void Initialize()
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

            _canvasGroup.gameObject.InActivate();
            _canvasGroup.gameObject.Activate();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = true;
            _rectTransform.DOAnchorPos(Vector2.zero, animDuration);
            _canvasTween?.Kill();
            _canvasTween = _canvasGroup.DOFade(1, animDuration).OnComplete(SetShow);
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
            _canvasTween?.Kill();
            _canvasTween = _canvasGroup.DOFade(0, animDuration / 5);
        }
        
        public virtual void ShowImmidiatly()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            isOpen = true;

            _rectTransform.anchoredPosition = Vector2.zero;
            _canvasGroup.gameObject.Activate();
        }

        public virtual void HideImmidiatly()
        {
            isOpen = false;
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.gameObject.InActivate();
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
            _canvasGroup.gameObject.InActivate();
            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = Vector2.zero;
            _canvasGroup.interactable = false;
            isOpen = false;
        }
    }
}
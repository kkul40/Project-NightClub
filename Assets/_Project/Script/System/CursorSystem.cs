using Disco_Building;
using HighlightPlus;
using PropBehaviours;
using UI.GamePages;
using UnityEngine;

namespace System
{
    public class CursorSystem : MonoBehaviour
    {
        [SerializeField] private HighlightProfile _interactableHighlight;
        [SerializeField] private HighlightProfile _propUnitHighlight;
        [SerializeField] private HighlightProfile _noneHighlight;
        
        private GameObject _currentGameObject;
        private IInteractable _currentInteractable;
        private InputSystem _inputSystem => InputSystem.Instance;

        private bool clicked;
        
        private void Update()
        {
            if (BuildingManager.Instance.isPlacing)
            {
                Reset();
                return;
            }

            if (clicked)
            {
                if (_inputSystem.CancelClick)
                {
                    clicked = false;
                    UIPageManager.Instance.CloseAPage(typeof(UIActionSelectionPage));
                    Reset();
                }
                else if (_inputSystem.LeftClickOnWorld)
                {
                    clicked = false;
                    UIPageManager.Instance.CloseAPage(typeof(UIActionSelectionPage));
                }
            }

            if (!clicked)
            {
                var hitTransform = _inputSystem.GetHitTransform();
                
                if (hitTransform == null)
                {
                    Reset();
                    return;
                }

                if (hitTransform.gameObject != _currentGameObject) Reset();

                if (hitTransform.TryGetComponent(out IInteractable cursorInteraction))
                {
                    if (cursorInteraction.IsInteractable)
                    {
                        if (_currentInteractable != cursorInteraction)
                            Set(cursorInteraction, hitTransform.gameObject);
                    }
                }
                
                if (_inputSystem.LeftClickOnWorld)
                    if (_currentInteractable != null && _currentInteractable.IsInteractable)
                        HandleOnClick();
            }
        }

        private void Set(IInteractable set, GameObject gameObject)
        {
            if (_currentInteractable == set) return;
            _currentInteractable = set;
            
            if (_currentInteractable != null)
            {
                if (!_currentInteractable.IsInteractable)
                {
                    Reset();
                    return;
                }
            }
            
            _currentGameObject = gameObject;
            _currentInteractable.OnFocus();

            // Add highligted
            var highlightEffect = _currentGameObject.GetComponent<HighlightEffect>();
            if (highlightEffect == null) highlightEffect = _currentGameObject.AddComponent<HighlightEffect>();

            switch (_currentInteractable.Interaction)
            {
                case eInteraction.None:
                    highlightEffect.ProfileLoad(_noneHighlight);
                    break;
                case eInteraction.PropUnit:
                    highlightEffect.ProfileLoad(_propUnitHighlight);
                    break;
                case eInteraction.Interactable:
                    highlightEffect.ProfileLoad(_interactableHighlight);
                    break;
                case eInteraction.Customer:
                    highlightEffect.ProfileLoad(_interactableHighlight);
                    break;
            }

            highlightEffect.SetHighlighted(true);
        }
        
        private void Reset()
        {
            if (_currentInteractable == null) return;

            _currentInteractable.OnOutFocus();
            _currentInteractable = null;

            if (_currentGameObject == null) return;

            // Remove hightlilghted
            var highlightEffect = _currentGameObject.GetComponent<HighlightEffect>();
            if (highlightEffect != null)
            {
                highlightEffect.SetHighlighted(false);
                Destroy(highlightEffect);
            }

            _currentGameObject = null;
        }

        private void HandleOnClick()
        {
            _currentInteractable.OnClick();
            clicked = true;
        }
    }
}
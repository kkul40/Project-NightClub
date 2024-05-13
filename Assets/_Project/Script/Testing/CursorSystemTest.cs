using System;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Serialization;

namespace Testing
{
    public class CursorSystemTest : MonoBehaviour
    {
        public HighlightProfile _interactableHighlight;
        public HighlightProfile _noneHighlight;
        private InputSystem _inputSystem => InputSystem.Instance;

        private IInteractable _currentInteractable = null;
        private GameObject _currentGameObject;

        private void Update()
        {
            if (BuildingSystem.Instance.GetPlacingType != PlacingType.None) return;
            
            Transform hitTransform = _inputSystem.GetMouseHitTransfromOnWorld();

            if (hitTransform == null)
            {
                Reset();
                return;
            }
            else if (hitTransform.gameObject != _currentGameObject)
            {
                Reset();
            }
            
            if (hitTransform.TryGetComponent(out IInteractable cursorInteraction))
            {
                if (_currentInteractable != cursorInteraction)
                {
                    Set(cursorInteraction, hitTransform.gameObject);
                }
            }

            if (_inputSystem.LeftClickOnWorld)
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.OnClick();
                }
            }
        }

        private void Reset()
        {
            if (_currentInteractable == null) return;
            
            _currentInteractable.OnOutFocus();
            _currentInteractable = null;
            
            // Remove hightlilghted
            HighlightEffect highlightEffect = _currentGameObject.GetComponent<HighlightEffect>();
            if (highlightEffect != null)
            {
                highlightEffect.SetHighlighted(false);
                Destroy(highlightEffect);
            }
            
            _currentGameObject = null;
        }

        private void Set(IInteractable set, GameObject gameObject)
        {
            if (_currentInteractable == set) return;
            
            _currentInteractable = set;
            _currentGameObject = gameObject;
            _currentInteractable.OnFocus();
            
            // Add highligted
            HighlightEffect highlightEffect = _currentGameObject.GetComponent<HighlightEffect>();
            if (highlightEffect == null)
            {
                highlightEffect = _currentGameObject.AddComponent<HighlightEffect>();
            }

            switch (_currentInteractable.Interaction)
            {
                case eInteraction.None:
                    highlightEffect.ProfileLoad(_noneHighlight);
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
    }
}
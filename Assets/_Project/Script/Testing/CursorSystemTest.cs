using System;
using BuildingSystem;
using HighlightPlus;
using PropBehaviours;
using UnityEngine;

namespace Testing
{
    public class CursorSystemTest : MonoBehaviour
    {
        public HighlightProfile _interactableHighlight;
        public HighlightProfile _propUnitHighlight;
        public HighlightProfile _noneHighlight;
        private GameObject _currentGameObject;

        private IInteractable _currentInteractable;
        private InputSystem _inputSystem => InputSystem.Instance;

        private void Reset()
        {
            if (_currentInteractable == null || BuildingManager.Instance.isPlacing) return;
            
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

        private void Update()
        {
            if (BuildingManager.Instance.isPlacing)
            {
                Reset();
                return;
            }

            var hitTransform = _inputSystem.GetHitTransform();

            if (hitTransform == null)
            {
                Reset();
                return;
            }

            if (hitTransform.gameObject != _currentGameObject) Reset();

            if (hitTransform.TryGetComponent(out IInteractable cursorInteraction))
                if (_currentInteractable != cursorInteraction)
                    Set(cursorInteraction, hitTransform.gameObject);

            if (_inputSystem.LeftClickOnWorld)
                if (_currentInteractable != null)
                    _currentInteractable.OnClick();
        }

        private void Set(IInteractable set, GameObject gameObject)
        {
            if (_currentInteractable == set) return;

            _currentInteractable = set;
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
    }
}
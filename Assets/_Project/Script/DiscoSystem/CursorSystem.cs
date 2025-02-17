using Disco_Building;
using ExtensionMethods;
using HighlightPlus;
using PropBehaviours;
using UI.GamePages;
using UnityEngine;

namespace DiscoSystem
{
    public class CursorSystem : MonoBehaviour
    {
        private enum CursorState
        {
            Free,
            Selected,
        }
        
        [SerializeField] private HighlightProfile _interactableHighlight;
        [SerializeField] private HighlightProfile _propUnitHighlight;
        [SerializeField] private HighlightProfile _selectionHightlight;
        [SerializeField] private HighlightProfile _noneHighlight;
        
        private GameObject _currentGameObject;
        private IInteractable _currentInteractable;

        private CursorState _cursorState = CursorState.Free;
        private InputSystem _inputSystem => InputSystem.Instance;

        private bool _isCursorLocked = false;
        private bool delaySelection = false;
        private float delayTimer = 0;

        private void OnEnable()
        {
            UISettingsPage.OnUISettingsToggle += ToggleCursorLock;
        }

        private void OnDisable()
        {
            UISettingsPage.OnUISettingsToggle -= ToggleCursorLock;
        }

        private void Update()
        {
            return;
            if (BuildingManager.Instance.isPlacing || _isCursorLocked)
            {
                Reset();
                delaySelection = true;
                return;
            }

            if (delaySelection)
            {
                delayTimer += Time.deltaTime;
                
                if (delayTimer >= 0.2f)
                {
                    delaySelection = false;
                    delayTimer = 0;
                }
                
                return;
            }
            
            var hitTransform = _inputSystem.GetHitTransform();
            
            switch (_cursorState)
            {
                case CursorState.Free:
                
                    if (hitTransform == null)
                    {
                        Reset();
                        return;
                    }

                    if (hitTransform.gameObject != _currentGameObject) Reset();

                    if (hitTransform.TryGetComponent(out IInteractable cursorInteraction))
                    {
                        if (_inputSystem.LeftClickOnWorld && cursorInteraction.IsInteractable && cursorInteraction.Interaction != eInteraction.None)
                        {
                            Set(cursorInteraction, hitTransform.gameObject, true);
                            OnClickHandler(cursorInteraction);
                            _cursorState = CursorState.Selected;
                        }
                        else
                        {
                            if (_currentInteractable != cursorInteraction)
                                Set(cursorInteraction, hitTransform.gameObject);
                        }
                    }
                    break;
                case CursorState.Selected:
                    if (_inputSystem.LeftClickOnWorld)
                    {
                        if (hitTransform.gameObject == _currentGameObject)
                        {
                            Reset();
                            _cursorState = CursorState.Free;
                        }
                        else
                        {
                            var interaction = hitTransform.GetComponent<IInteractable>();
                            
                            if (interaction != null && interaction.IsInteractable && interaction.Interaction != eInteraction.None)
                            {
                                if (_currentInteractable != interaction)
                                {
                                    Reset();
                                    Set(interaction, hitTransform.gameObject, true);
                                    OnClickHandler(_currentInteractable);
                                }
                            }
                            else
                            {
                                Reset();
                                _cursorState = CursorState.Free;
                            }
                        }
                    }
                    else if (_inputSystem.CancelClick)
                    {
                        Reset();
                        _cursorState = CursorState.Free;
                    }
                    break;
            }
        }

        private void Set(IInteractable set, GameObject gameObject, bool selection = false)
        {
            if (_currentInteractable == set && !selection) return;
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
            
            if(!selection)
                _currentInteractable.OnFocus();

            // Add highligted
            var highlightEffect = _currentGameObject.GetComponent<HighlightEffect>();
            if (highlightEffect == null) highlightEffect = _currentGameObject.AddComponent<HighlightEffect>();

            if (selection)
            {
                highlightEffect.ProfileLoad(_selectionHightlight);
                delaySelection = true;
            }
            else
            {
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

        private void OnClickHandler(IInteractable interactable)
        {
            interactable.OnClick();
            SFXPlayer.Instance.PlaySoundEffect(SFXPlayer.Instance.Click);
            if(interactable.IsAnimatable)
                interactable.mGameobject.AnimatedPlacement(ePlacementAnimationType.Shaky);
        }
        public void ToggleCursorLock(bool toggle) => _isCursorLocked = toggle;
    }
}
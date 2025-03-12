using System.Collections;
using DiscoSystem;
using ExtensionMethods;
using GameEvents;
using HighlightPlus;
using PropBehaviours;
using UI.GamePages;
using UnityEngine;

namespace System
{
    public class CursorSystem : MonoBehaviour
    {
        public enum eCursorTypes
        {
            Default,
            Building,
            Dragging,
            Rotating,
            Settings,
        }
        private enum CursorState
        {
            Free,
            Selected,
        }
        
        public Texture2D DefaultCursor;
        public Texture2D BuildingCursor;
        public Texture2D DraggingCursor;
        public Texture2D RotatingCursor;
        public Texture2D SettingsCursor;

        private eCursorTypes currentCursor = eCursorTypes.Default;
        private eCursorTypes previousCursor = eCursorTypes.Default;
        
        [SerializeField] private HighlightProfile _interactableHighlight;
        [SerializeField] private HighlightProfile _propUnitHighlight;
        [SerializeField] private HighlightProfile _selectionHightlight;
        [SerializeField] private HighlightProfile _noneHighlight;
        
        private GameObject _currentGameObject;
        private IInteractable _currentInteractable;

        private CursorState _cursorState = CursorState.Free;
        private InputSystem _inputSystem;

        private bool _isCursorSystemToggled = false;

        public void Initialize(InputSystem inputSystem ,float delay)
        {
            _inputSystem = inputSystem;
            SetCursor(eCursorTypes.Default);
            StartCoroutine(ToggleCursorSystem(true, delay));
        }

        private void OnEnable()
        {
            KEvent_Cursor.OnChangeCursor += SetCursor;
            KEvent_Cursor.OnChangeCursorToPrevious += SetToPreviousChangeCursorTo;
            KEvent_Building.OnBuildingToggled += ToggleCursorSystem;
            KEvent_Cursor.OnResetSelection += Reset;
        }

        private void OnDisable()
        {
            KEvent_Cursor.OnChangeCursor -= SetCursor;
            KEvent_Cursor.OnChangeCursorToPrevious -= SetToPreviousChangeCursorTo;
            KEvent_Building.OnBuildingToggled -= ToggleCursorSystem;
            KEvent_Cursor.OnResetSelection -= Reset;
        }
 
        private void Update()
        {
            if (!_isCursorSystemToggled)
            {
                Reset();
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
            _currentInteractable.OnDeselect();
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
            KEvent_SoundFX.TriggerSoundFXPlay(SoundFXType.Click);
            if(interactable.hasInteractionAnimation)
                interactable.mGameobject.AnimatedPlacement(ePlacementAnimationType.Shaky);
        }

        public void SetCursor(eCursorTypes cursorType)
        {
            switch (cursorType)
            {
                case eCursorTypes.Default:
                    Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case eCursorTypes.Building:
                    Cursor.SetCursor(BuildingCursor, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case eCursorTypes.Dragging:
                    Cursor.SetCursor(DraggingCursor, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case eCursorTypes.Rotating:
                    Cursor.SetCursor(RotatingCursor, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case eCursorTypes.Settings:
                    Cursor.SetCursor(SettingsCursor, Vector2.zero, CursorMode.ForceSoftware);
                    break;
            }

            previousCursor = currentCursor;
            currentCursor = cursorType;
        }

        public void SetToPreviousChangeCursorTo()
        {
            currentCursor = previousCursor;
            SetCursor(currentCursor);
        }
        public void ToggleCursorSystem(bool toggle)
        {
            StartCoroutine(ToggleCursorSystem(!toggle, 0.1f));
        }

        private IEnumerator ToggleCursorSystem(bool toggle, float delay)
        {
            yield return new WaitForSeconds(delay);
            _isCursorSystemToggled = toggle;
        }
    }
}
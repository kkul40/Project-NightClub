using System;
using UnityEngine;

namespace Testing
{
    public class CursorSystemTest : MonoBehaviour
    {
        private InputSystem _inputSystem => InputSystem.Instance;

        private ICursorInteraction _currentCursorInteraction;

        private void Update()
        {
            if (BuildingSystem.Instance.GetPlacingType != PlacingType.None) return;
            
            Transform hitTransform = _inputSystem.GetMouseHitTransfromOnWorld();

            if (hitTransform == null)
            {
                Reset();
                return;
            }
            
            if (hitTransform.TryGetComponent(out ICursorInteraction cursorInteraction))
            {
                if (_currentCursorInteraction != cursorInteraction)
                {
                    Set(cursorInteraction);
                }
            }
            else
            {
                Reset();
            }

            if (_inputSystem.LeftClickOnWorld)
            {
                if (_currentCursorInteraction != null)
                {
                    _currentCursorInteraction.OnClick();
                }
            }
        }

        private void Reset()
        {
            if (_currentCursorInteraction == null) return;
            
            _currentCursorInteraction.OnOutFocus();
            _currentCursorInteraction = null;
        }

        private void Set(ICursorInteraction set)
        {
            if (_currentCursorInteraction == set) return;
            
            _currentCursorInteraction = set;
            _currentCursorInteraction.OnFocus();
        }
    }
}
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DiscoSystem
{
    public enum InputType
    {
        WasPerformedThisFrame,
        WasPressedThisFrame,
        WasReleasedThisFrame,
        InProggress,
    }
    
    [DisallowMultipleComponent]
    public class InputSystem : Singleton<InputSystem>
    {
        [SerializeField] private Camera mainCam;
        private RaycastHit lastHit;
        private Vector3Int MouseLastCellPosition;
        
        [Header("Toggle")] 
        public bool EdgeScrolling;
        public bool SnappyCamera;
        [Header("")]

        [SerializeField] private float smoothTime;

        [SerializeField] private LayerMask mouseOverLayers;
        [SerializeField] private LayerMask ignore;
        [SerializeField] private float borderTreshold = 0.01f;

        public Vector3 MousePosition;
        public bool HasMouseMoveToNewCell; // Used For Optimization

        private Vector2 _cameraMoveDelta = Vector2.zero;        
        // INPUTS
        private bool _inputEnabled = true;
        
        public PlayerInput _input;
        
        private InputAction _esc;
        private InputAction _undo;
        private InputAction _rotate;
        private InputAction _zoom;
        private InputAction _leftClick;
        private InputAction _rigthClick;
        private InputAction _cameraMovement;
        private InputAction _freePlacementKey;
        private InputAction _mousePosition;
        
        public void Initialize()
        {
            _esc = _input.actions["ESC"];
            _undo = _input.actions["Undo"];
            _rotate = _input.actions["Rotate"];
            _zoom = _input.actions["Zoom"];
            _leftClick = _input.actions["LeftClick"];
            _rigthClick = _input.actions["RightClick"];
            _cameraMovement = _input.actions["CameraMovement"];
            _freePlacementKey = _input.actions["FreePlacementKey"];
            _mousePosition = _input.actions["MousePosition"];
            
            GameEvent.Subscribe<Event_ToggleInputs>(handle =>
            {
                if(handle.Toggle)
                    EnableInputs();
                else
                    DisableInputs();
            });
        }

        private void Update()
        {
            MousePosition = GetMouseMapPosition();
        }

        public Vector2 GetCameraMoveDelta()
        {
            if (!_inputEnabled) return Vector2.zero;
            
            Vector2 rawInput = _cameraMovement.ReadValue<Vector2>();

            _cameraMoveDelta = Vector2.Lerp(_cameraMoveDelta, rawInput, Time.deltaTime / smoothTime);
            
            if (SnappyCamera)
            {
                rawInput.Normalize();
                return rawInput / 2;
            }
            
            if (_cameraMoveDelta.sqrMagnitude > 1)
                return _cameraMoveDelta.normalized;

            return _cameraMoveDelta;
        }

        public bool GetLeftClickOnWorld(InputType inputType)
        {
            if (!_inputEnabled) return false;

            if (EventSystem.current.IsPointerOverGameObject()) return false;
            
            return GetActionType(_leftClick, inputType);
        }
        
        public bool GetRightClickOnWorld(InputType inputType)
        {
            if (!_inputEnabled) return false;

            if (EventSystem.current.IsPointerOverGameObject()) return false;
            
            return GetActionType(_rigthClick, inputType);
        }

        public bool GetFreePlacement(InputType inputType)
        {
            if (!_inputEnabled) return false;

            return GetActionType(_freePlacementKey, inputType);
        }

        public bool GetRotation(InputType inputType)
        {
            if (!_inputEnabled) return false;

            if (GetActionType(_rotate, inputType)) return true;

            return false;
        }
        public int GetRotation()
        {
            if (!_inputEnabled) return 0;
            
            return (int)_rotate.ReadValue<float>();
        } 

        public bool GetCancel(InputType inputType)
        {
            if (GetActionType(_rigthClick, inputType)) return true;
            if (GetActionType(_esc, inputType)) return true;

            return false;
        }

        public bool GetEscape(InputType inputType)
        {
            return GetActionType(_esc, inputType);
        }

        public float GetZoomDelta()
        {
            if (!_inputEnabled) return 0;
            
            return _zoom.ReadValue<float>();
        }

        public bool Undo(InputType inputType)
        {
            if (!_inputEnabled) return false;
            
            return GetActionType(_undo, inputType);
        }

        private bool GetActionType(InputAction action, InputType actionType)
        {
            switch (actionType)
            {
                case InputType.WasPerformedThisFrame:
                    return action.WasPerformedThisFrame();
                case InputType.WasPressedThisFrame:
                    return action.WasPressedThisFrame();
                case InputType.WasReleasedThisFrame:
                    return action.WasReleasedThisFrame();
                case InputType.InProggress:
                    return action.inProgress;
            }
            
            Debug.LogError("Input Action Type Not Found!!!");
            return false;
        }

        public Vector2 GetEdgeScrollingData()
        {
            if (!_inputEnabled) return Vector2.zero;
            
            Vector2 vector = Vector2.zero;
            
            if (!EdgeScrolling) return vector;
            
            var viewport = mainCam.ScreenToViewportPoint(Input.mousePosition);
        
            if (viewport.x <= borderTreshold && viewport.y <= borderTreshold) // sol alt
            {
                vector = new Vector2(-1, -1);
            }
            else if(viewport.x <= borderTreshold && viewport.y >= 1 - borderTreshold) // sol ust
            {
                vector = new Vector2(-1, 1);
            }
            else if (viewport.x >= 1 - borderTreshold && viewport.y <= borderTreshold)// sag alt
            {
                vector = new Vector2(1, -1);
            }
            else if (viewport.x >= 1 - borderTreshold && viewport.y >= 1 - borderTreshold) // sag ust
            {
                vector = new Vector2(1, 1);
            }
            else if (viewport.x <= borderTreshold)
            {
                vector = new Vector2(-1, 0);
            }
            else if (viewport.x >= 1 - borderTreshold)
            {
                vector = new Vector2(1, 0);
            }
            else if (viewport.y <= borderTreshold)
            {
                vector = new Vector2(0, -1);
            }
            else if (viewport.y >= 1 - borderTreshold)
            {
                vector = new Vector2(0, 1);
            }

            return vector.normalized;
        }

        private Vector3 GetMouseMapPosition()
        {
            if (!_inputEnabled) return Vector3.zero;
            
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            if (Physics.Raycast(ray, out hit, maxDistance, mouseOverLayers))
            {
                var hitPointCellPosition = hit.point.WorldPosToCellPos(eGridType.PlacementGrid);
                if (MouseLastCellPosition != hitPointCellPosition)
                {
                    MouseLastCellPosition = hitPointCellPosition;
                    HasMouseMoveToNewCell = true;
                }
                else
                {
                    HasMouseMoveToNewCell = false;
                }
                
                lastHit = hit;
                return hit.point;
            }
            
            HasMouseMoveToNewCell = false;
            return Vector3.zero;
        }

        public Vector3 GetMousePositionOnLayer(int layerID)
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            if (Physics.Raycast(ray, out hit, maxDistance, 1 << layerID))
                return hit.point;
            
            return Vector3.zero;
        }

        /// <summary>
        /// Uses Ignore Layer To Ignore Certain Objects.
        /// </summary>
        /// <returns></returns>
        public Transform GetHitTransform()
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            if (Physics.Raycast(ray, out hit, maxDistance, ~ignore)) return hit.transform;

            return null;
        }

        public Transform GetHitTransformWithLayer(int layerID)
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            if (Physics.Raycast(ray, out hit, maxDistance, 1 << layerID)) return hit.transform;
            return null;
        }

        public void EnableInputs() => _inputEnabled = true;
        public void DisableInputs() => _inputEnabled = false;
    }
}
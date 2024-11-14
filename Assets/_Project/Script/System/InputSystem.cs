using Data;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace System
{
    [DisallowMultipleComponent]
    public class InputSystem : Singleton<InputSystem>
    {
        [SerializeField] private Camera mainCam;
        private RaycastHit lastHit;
        private Vector3Int MouseLastCellPosition;
        
        [Header("Toggle")] 
        public bool EdgeScrolling;
        [Header("")]

        [SerializeField] private LayerMask mouseOverLayers;
        [SerializeField] private LayerMask ignore;
        [SerializeField] private float borderTreshold = 0.01f;

        public Vector2 MoveDelta;
        public float ScrollWheelDelta;
        public Vector3 MousePosition;
        [HideInInspector] public bool Esc;
        [HideInInspector] public bool TurnLeft;
        [HideInInspector] public bool TurnRight;
        [HideInInspector] public bool IsMouseCursorOnWorld;
        [HideInInspector] public bool LeftClickOnWorld;
        [HideInInspector] public bool LeftHoldClickOnWorld;
        [FormerlySerializedAs("RightClick")] public bool CancelClick;
        public bool RightClickOnWorld;

        public bool HasMouseMoveToNewCell; // Used For Optimization

        private void Update()
        {
            MoveDelta.x = Input.GetAxis("Horizontal");
            MoveDelta.y = Input.GetAxis("Vertical");
            
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                LeftClickOnWorld = Input.GetMouseButtonDown(0);
                LeftHoldClickOnWorld = Input.GetMouseButton(0);
                RightClickOnWorld = Input.GetMouseButtonDown(1);
                ScrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
                IsMouseCursorOnWorld = true;
            }
            else
            {
                LeftHoldClickOnWorld = false;
                IsMouseCursorOnWorld = false;
            }

            CancelClick = Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape);

            MousePosition = GetMouseMapPosition();
            Esc = Input.GetKeyDown(KeyCode.Escape);
            TurnLeft = Input.GetKeyDown(KeyCode.Z);
            TurnRight = Input.GetKeyDown(KeyCode.X);
        }

        public Vector2 GetEdgeScrollingData()
        {
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

        public Transform GetHitTransform()
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            // TODO Ignore Layer Ekle ki kapi bugg da kalmasin
            // int ignoreLayer = ignore;
            // int ignoreMask = ~(1 << ignoreLayer);

            if (Physics.Raycast(ray, out hit, maxDistance)) return hit.transform;

            return null;
        }

        public Transform GetHitTransformWithLayer(int layerID)
        {
            LayerMask layer = 1 << layerID;
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            if (Physics.Raycast(ray, out hit, maxDistance, layer)) return hit.transform;
            return null;
        }
    }
}
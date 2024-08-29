using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace System
{
    [DisallowMultipleComponent]
    public class InputSystem : Singleton<InputSystem>
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private RaycastHit lastHit;

        [SerializeField] private LayerMask mouseOverLayers;
        [SerializeField] private LayerMask ignore;

        public Vector2 MoveDelta;
        public float ScrollWheelDelta;
        [HideInInspector] public bool Esc;
        [HideInInspector] public bool TurnLeft;
        [HideInInspector] public bool TurnRight;
        [HideInInspector] public bool IsMouseCursorOnWorld;
        [HideInInspector] public bool LeftClickOnWorld;
        [HideInInspector] public bool LeftHoldClickOnWorld;
        public bool RightClickOnWorld;

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
                IsMouseCursorOnWorld = false;
            }

            Esc = Input.GetKeyDown(KeyCode.Escape);
            TurnLeft = Input.GetKeyDown(KeyCode.Z);
            TurnRight = Input.GetKeyDown(KeyCode.X);
        }

        public Vector3 GetMouseMapPosition()
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;

            if (Physics.Raycast(ray, out hit, maxDistance, mouseOverLayers))
            {
                lastHit = hit;
                return hit.point;
            }

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
using UnityEngine;
using UnityEngine.EventSystems;

namespace System
{
    [DisallowMultipleComponent]
    public class InputSystem : MonoBehaviour
    {
        public static InputSystem Instance;
        [SerializeField] private Camera mainCam;
        [SerializeField] private RaycastHit lastHit;
        [SerializeField] private Transform lastHitTransform;

        [SerializeField] private LayerMask mouseOverLayers;
        [SerializeField] private LayerMask ignore;

        public Vector2 MoveDelta;
        public float ScrollWheelDelta;
        [HideInInspector] public bool Esc;
        [HideInInspector] public bool E;
        [HideInInspector] public bool Q;
        [HideInInspector] public bool LeftClickOnWorld;
        [HideInInspector] public bool LeftHoldClickOnWorld;
        public bool RightClickOnWorld;

        private void Awake()
        {
            Instance = this;
        }

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
            }

            Esc = Input.GetKeyDown(KeyCode.Escape);
            E = Input.GetKeyDown(KeyCode.E);
            Q = Input.GetKeyDown(KeyCode.Q);
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
                lastHitTransform = hit.transform;
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

            if (Physics.Raycast(ray, out hit, maxDistance, ignore)) return hit.transform;

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
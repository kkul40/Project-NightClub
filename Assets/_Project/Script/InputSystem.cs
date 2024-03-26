using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Script.NewSystem
{
    [DisallowMultipleComponent]
    public class InputSystem : MonoBehaviour
    {
        public static InputSystem Instance;
        [SerializeField] private Camera mainCam;
        [SerializeField] private RaycastHit lastHit;

        [SerializeField] private LayerMask placementLayer;
        [SerializeField] private LayerMask groundLayer;

        public Vector2 MoveDelta;
        public float ScrollWheelDelta;
        [HideInInspector] public bool Esc;
        [HideInInspector] public bool E;
        [HideInInspector] public bool Q;
        [HideInInspector] public bool LeftClickOnWorld;
        public bool RightClickOnWorld;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            MoveDelta.x = Input.GetAxis("Horizontal");
            MoveDelta.y = Input.GetAxis("Vertical");
            ScrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
            
            Esc = Input.GetKeyDown(KeyCode.Escape);
            E = Input.GetKeyDown(KeyCode.E);
            Q = Input.GetKeyDown(KeyCode.Q);
            LeftClickOnWorld = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
            RightClickOnWorld = Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject();
        }

        public Vector3 GetMouseMapPosition()
        {
            Vector3 mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            Ray ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 100;
            if (Physics.Raycast(ray, out hit, maxDistance, placementLayer))
            {
                lastHit = hit;
                return hit.point;
            }
            
            if(Physics.Raycast(ray, out hit, maxDistance, groundLayer))
            {
                lastHit = hit;
                return hit.point;
            }
            
            return Vector3.zero;
        }

        public RaycastHit GetLastHit() => lastHit;
    }
}
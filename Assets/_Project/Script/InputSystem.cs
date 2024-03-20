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
        
        public event Action OnClicked, OnExit;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                OnClicked?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExit?.Invoke();
            }
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
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Script.NewSystem
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private RaycastHit lastHit;

        [SerializeField]
        private LayerMask placementLayer;

        public event Action OnClicked, OnExit;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
            if (Physics.Raycast(ray, out hit, 100, placementLayer))
            {
                lastHit = hit;
            }
            return lastHit.point;
        }

        public RaycastHit GetLastHit() => lastHit;
    }
}
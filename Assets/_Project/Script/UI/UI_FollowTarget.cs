using System;
using _Initializer;
using DiscoSystem.CameraSystem;
using UI.GamePages;
using Unity.Mathematics;
using UnityEngine;

namespace UI
{
    public class UI_FollowTarget : MonoBehaviour
    {
        public Vector2 Offset;
        
        private GameObject targetObject;
        private Collider _targetCollider;

        private Vector3 centerPosition;
        private float3 _cameraMinMaxCurrentSize;

        private void Start()
        {
            _cameraMinMaxCurrentSize = ServiceLocator.Get<CameraControl>().GetCameraSize;
        }

        public void SetTarget(GameObject target)
        {
            targetObject = target;
            
            if (target == null) return;
            centerPosition = CalculateCenter(targetObject.GetComponents<Collider>());
           
            AnchorPos();
        }

        private void Update()
        {
            AnchorPos();
        }
        
        public void AnchorPos()
        {
            if (targetObject == null)
                return;

            Canvas _canvas = ServiceLocator.Get<UIPageManager>().GetCanvas;

            if (_canvas == null)
            {
                Debug.LogError("Canvas Can't Found Anywhere");
                return;
            }

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(centerPosition + new Vector3(Offset.x, 0, Offset.y));
        
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.GetComponent<RectTransform>(), screenPosition, _canvas.worldCamera, out canvasPosition);
        
            GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        }
        
        private Vector3 CalculateCenter(Collider[] colliders)
        {
            if (colliders.Length == 0)
            {
                Debug.LogWarning("No colliders assigned.");
                return Vector3.zero;
            }

            Vector3 cumulativeCenter = Vector3.zero;

            foreach (Collider col in colliders)
                cumulativeCenter += col.bounds.center;

            Vector3 center = cumulativeCenter / colliders.Length;

            return center;
        }
        
    }
}
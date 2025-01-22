using DiscoSystem;
using UI.GamePages;
using Unity.Mathematics;
using UnityEngine;

namespace UI
{
    public class UI_FollowTarget : MonoBehaviour
    {
        public Vector3 Offset;
        
        private Canvas _canvas;
        private GameObject targetObject;
        private Collider _targetCollider;

        private Vector3 centerPosition;
        private float3 _cameraMinMaxCurrentSize;

        private void Awake()
        {
            _canvas = UIPageManager.Instance.transform.GetComponent<Canvas>();
            _cameraMinMaxCurrentSize = CameraControl.Instance.GetCameraSize;
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

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(centerPosition);
        
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

            // Sum all collider center points
            foreach (Collider col in colliders)
            {
                cumulativeCenter += col.bounds.center;
            }

            // Divide by the number of colliders to find the average
            Vector3 center = cumulativeCenter / colliders.Length;

            return center;
        }
        
    }
}
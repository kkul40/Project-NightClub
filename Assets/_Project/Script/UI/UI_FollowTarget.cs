using System;
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
        private float3 _cameraMinMaxCurrentSize;

        private void Awake()
        {
            _canvas = UIPageManager.Instance.transform.GetComponent<Canvas>();
            _cameraMinMaxCurrentSize = CameraControl.Instance.GetCameraSize;
        }

        public void SetTarget(GameObject target)
        {
            targetObject = target;
            AnchorPos();
        }

        private void Update()
        {
            float minZoom = _cameraMinMaxCurrentSize.x;
            float maxZoom = _cameraMinMaxCurrentSize.y;
            float cameraSize = _cameraMinMaxCurrentSize.z;
            AnimationCurve zoomCurve = CameraControl.Instance.GetAnimationCurve;

            float scaleMultiplier = Mathf.Clamp(
                maxZoom - (cameraSize - minZoom), 
                minZoom, 
                maxZoom
            );

            float targetScale = zoomCurve.Evaluate(Mathf.InverseLerp(maxZoom, 1, scaleMultiplier));

            transform.localScale = Mathf.LerpUnclamped(
                transform.localScale.x, 
                targetScale, 
                Time.deltaTime * 20
            ) * Vector3.one;
            
            AnchorPos();
        }
        
        public void AnchorPos()
        {
            if (targetObject == null)
                return;
        
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position + Offset);
        
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.GetComponent<RectTransform>(), screenPosition, _canvas.worldCamera, out canvasPosition);
        
            GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        }
        
    }
}
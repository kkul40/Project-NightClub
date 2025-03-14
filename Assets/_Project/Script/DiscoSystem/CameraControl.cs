using System;
using System.Music;
using GameEvents;
using Unity.Mathematics;
using UnityEngine;

namespace DiscoSystem
{
    public class CameraControl : Singleton<CameraControl>
    {
        [SerializeField] private Camera mainCam;

        [SerializeField] private float speed;
        [SerializeField] private float zoomMultiplier;
        [SerializeField] private AnimationCurve _zoomAnimationCurve;


        [SerializeField] private Vector3 _followNPCOffset;
        
        [Range(1, 9)] [SerializeField] private float cameraSize = 5;

        private Vector3 nextPosition;
        private float timeElapsed = 0;

        private Transform _target;

        private void LateUpdate()
        {
            var moveDelta = InputSystem.Instance.MoveDelta;

            if (InputSystem.Instance.GetEdgeScrollingData() != Vector2.zero)
            {
                moveDelta = InputSystem.Instance.GetEdgeScrollingData();
            }

            if (moveDelta.magnitude > 1) moveDelta = moveDelta.normalized;

            if (moveDelta != Vector2.zero) _target = null;

            if (_target != null)
            {
                Vector3 npcPos = _target.transform.position;
                nextPosition = Vector3.Lerp(nextPosition, new Vector3(npcPos.x, 0, npcPos.z) + _followNPCOffset,
                    speed * Time.deltaTime);
            }
            else
            {
                nextPosition = transform.position + (transform.forward * moveDelta.y + transform.right * moveDelta.x) *
                    (speed * Time.deltaTime);
            }
           

            transform.position = nextPosition;

            SetCameraSize();
        }

        private void SetCameraSize()
        {
            cameraSize -= InputSystem.Instance.ScrollWheelDelta * zoomMultiplier;
            
            cameraSize = Mathf.Clamp(cameraSize, 1, 9);
            mainCam.orthographicSize =
                Mathf.Lerp(mainCam.orthographicSize, cameraSize, Time.deltaTime * zoomMultiplier * 2);
        }

        public float3 GetCameraSize => new float3(1, 9, cameraSize);
        public AnimationCurve GetAnimationCurve => _zoomAnimationCurve;

        public void FollowTarget(Transform target)
        {
            _target = target;
            GameEvent.Trigger(new Event_Sfx(SoundFXType.CameraFocus));
        }

        public void ResetTarget()
        {
            _target = null; 
        }
    }
}
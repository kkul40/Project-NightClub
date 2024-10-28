using UnityEngine;

namespace System
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;

        [SerializeField] private float speed;
        [SerializeField] private float zoomMultiplier;

        [Range(1, 9)] [SerializeField] private float cameraSize = 5;

        private float timeElapsed = 0;

        private void LateUpdate()
        {
            var moveDelta = InputSystem.Instance.MoveDelta;

            if (InputSystem.Instance.GetEdgeScrollingData() != Vector2.zero)
            {
                moveDelta = InputSystem.Instance.GetEdgeScrollingData();
            }

            if (moveDelta.magnitude > 1) moveDelta = moveDelta.normalized;

            var nextPos = transform.position + (transform.forward * moveDelta.y + transform.right * moveDelta.x) *
                (speed * Time.deltaTime);

            transform.position = nextPos;

            SetCameraSize();
        }

        private void SetCameraSize()
        {
            cameraSize -= InputSystem.Instance.ScrollWheelDelta * zoomMultiplier;
            cameraSize = Mathf.Clamp(cameraSize, 1, 9);
            mainCam.orthographicSize =
                Mathf.Lerp(mainCam.orthographicSize, cameraSize, Time.deltaTime * zoomMultiplier * 2);
        }
    }
}
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    [SerializeField] private float speed;
    [SerializeField] private float zoomMultiplier;

    [Range(2,9)]
    [SerializeField] private int cameraSize = 5;

    private float timeElapsed = 0;


    private void LateUpdate()
    {
        var moveDelta = Vector3.zero;
        moveDelta.x = Input.GetAxis("Horizontal");
        moveDelta.z = Input.GetAxis("Vertical");

        if (moveDelta.magnitude > 1)
        {
            moveDelta = moveDelta.normalized;
        }

        transform.position += (transform.forward * moveDelta.z + transform.right * moveDelta.x) * speed * Time.deltaTime;
        SetCameraSize();
    }

    private void SetCameraSize()
    {
        cameraSize -= (int)(Input.GetAxis("Mouse ScrollWheel") * 10);
        cameraSize = Mathf.Clamp(cameraSize, 2, 9);
        mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, cameraSize, Time.deltaTime * zoomMultiplier * 2);
    }
}

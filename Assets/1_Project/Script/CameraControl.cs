using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    [SerializeField] private float speed;


    private void Update()
    {
        var moveDelta = Vector3.zero;
        moveDelta.x = Input.GetAxis("Horizontal");
        moveDelta.z = Input.GetAxis("Vertical");

        if (moveDelta.magnitude > 1)
        {
            moveDelta = moveDelta.normalized;
        }

        transform.position += (transform.forward * moveDelta.z + transform.right * moveDelta.x) * speed * Time.deltaTime;
        
        
        
        
    }
}

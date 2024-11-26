using UnityEngine;

namespace UI
{
    public class UIRotater : MonoBehaviour
    {
        public float speed;
        private void Update()
        {
            transform.eulerAngles += new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}
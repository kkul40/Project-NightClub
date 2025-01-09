using UnityEngine;

namespace CharacterCustomization.UI
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
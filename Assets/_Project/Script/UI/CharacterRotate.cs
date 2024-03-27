using UnityEngine;

namespace CharacterCreation
{
    public class CharacterRotate : MonoBehaviour
    {
        [SerializeField] private Transform characterTransform;
        [SerializeField] private float ratateAmount;
    
        public void OnLeftButton()
        {
            characterTransform.Rotate(Vector3.up * ratateAmount);
        }

        public void OnRightButton()
        {
            characterTransform.Rotate(Vector3.up * -ratateAmount);
        }

        public void DragRotate()
        {
            float rotX = Input.GetAxis("Mouse X") * 10;
        
            characterTransform.Rotate(Vector3.up * -rotX);
        }
    }
}

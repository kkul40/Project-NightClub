using UnityEngine;

namespace Prop_Behaviours
{
    public class OnlyEmployeeColider : MonoBehaviour
    {
        public Collider onlyEmployeeCollider;

        private void Awake()
        {
            onlyEmployeeCollider.isTrigger = true;
        }
    }
}
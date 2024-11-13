using UnityEngine;

namespace System
{
    public class IPooled : MonoBehaviour
    {
        public bool IsActive = false;
        public float RemainingTime;

        private ObjectPooler Pool;

        public void Init(ObjectPooler pooler,float remaningTime)
        {
            Pool = pooler;
            RemainingTime = remaningTime;
            IsActive = true;
        }

        public void ReturnToPool()
        {
            IsActive = false;
            RemainingTime = 0;
            gameObject.SetActive(false);
        }
    }
}
using _Initializer;
using DiscoSystem.Building_System;
using UnityEngine;

namespace DiscoSystem.ObjectPooling
{
    public interface IPoolable
    {
        Transform mTransform { get; set; }
        bool IsActive { get; set; }
        float RemainingTime { get; set; }

        public ObjectPooler Pool { get; set; }

        public void Init(ObjectPooler pooler, float remainingTime)
        {
            Pool = pooler;
            RemainingTime = remainingTime;
            IsActive = true;
        }
        
        public void ReturnToPool()
        {
            IsActive = false;
            RemainingTime = 0;
            mTransform.gameObject.SetActive(false);
            mTransform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().PooledObjectHolder);
        }
    }
}
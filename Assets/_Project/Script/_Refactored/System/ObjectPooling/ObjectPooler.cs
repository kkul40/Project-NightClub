using System.Building;
using System.Collections.Generic;
using DiscoSystem;
using UnityEngine;

namespace System.ObjectPooling
{
    public class ObjectPooler : IUpdateable
    {
        public List<IPoolable> _pooledObjects;

        private GameObject _prefab;
        
        public ObjectPooler(GameObject prefab)
        {
            _pooledObjects = new List<IPoolable>();
            _prefab = prefab;
            

            for (int i = 0; i < 10; i++)
                CreateNew();
            
            UpdatableHandler.Instance.RegisterUpdate(this);
        }

        public void TickUpdate(float deltaTime)
        {
            foreach (var pooledObject in _pooledObjects)
            {
                if(!pooledObject.IsActive) continue;

                pooledObject.RemainingTime -= deltaTime;
                if (pooledObject.RemainingTime <= 0)
                {
                    pooledObject.ReturnToPool();
                }
            }
        }

        public GameObject GetObject(float disposeTime)
        {
            IPoolable selected = null;
            foreach (var pooledObject in _pooledObjects)
            {
                if (!pooledObject.mTransform.gameObject.activeInHierarchy)
                {
                    selected = pooledObject;
                    break;
                }
            }

            if(selected == null)
                selected = CreateNew();
            
            selected.Init(this, disposeTime);
            selected.mTransform.gameObject.SetActive(true);
            return selected.mTransform.gameObject;
        }
        
        private IPoolable CreateNew()
        {
            var obj = MonoBehaviour.Instantiate(_prefab, SceneGameObjectHandler.Instance.PooledObjectHolder);
            var pooledObject = obj.GetComponent<IPoolable>();
            obj.SetActive(false);

            if (pooledObject == null)
            {
                Debug.LogError($"Could Not Found IPollable Objects {obj.name}");
            }
            _pooledObjects.Add(pooledObject);
            return pooledObject;
        }

        private void ReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
using System.Collections.Generic;
using Disco_Building;
using UnityEngine;

namespace System.ObjectPooling
{
    public class ObjectPooler : IUpdateable
    {
        public List<IPooled> _pooledObjects;

        private GameObject _prefab;
        
        public ObjectPooler(GameObject prefab)
        {
            _pooledObjects = new List<IPooled>();
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
            IPooled selected = null;
            foreach (var pooledObject in _pooledObjects)
            {
                if (!pooledObject.gameObject.activeInHierarchy)
                {
                    selected = pooledObject;
                    break;
                }
            }

            if(selected == null)
                selected = CreateNew();
            
            selected.Init(this, disposeTime);
            selected.gameObject.SetActive(true);
            return selected.gameObject;
        }
        
        private IPooled CreateNew()
        {
            var obj = MonoBehaviour.Instantiate(_prefab, SceneGameObjectHandler.Instance.PooledObjectHolder);
            var pooledObject = obj.AddComponent<IPooled>();
            obj.SetActive(false);
            _pooledObjects.Add(pooledObject);
            return pooledObject;
        }

        private void ReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
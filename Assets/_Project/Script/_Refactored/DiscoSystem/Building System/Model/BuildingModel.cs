using System;
using Data;
using Disco_ScriptableObject;
using Framework.Mvcs.Model;
using UnityEngine;

namespace DiscoSystem.Building_System.Model
{
    public class BuildingModel : BaseModel
    {
        public void AddPlacmeentItem(StoreItemSO itemSo, Transform sceneObject, Vector3 placedPosition, Quaternion placedRotation, Vector3 colliderSize)
        {
            DiscoData.Instance.PlacedItems.Add(sceneObject.GetInstanceID(), new Tuple<int, Transform, Vector3, Quaternion, Vector3>(itemSo.ID, sceneObject, placedPosition, placedRotation, colliderSize));
        }

        public void RemovePlacementItem(int ID)
        {
            DiscoData.Instance.PlacedItems.Remove(ID);
        }

        public Transform GetPlacedSceneObjectByID(int ID)
        {
            return DiscoData.Instance.PlacedItems[ID].Item2;
        }

        public Vector3 GetColliderSizeOfPlacedObject(int ID)
        {
            return DiscoData.Instance.PlacedItems[ID].Item5;
        }

        public StoreItemSO GetStoreItemByID(int instanceID)
        {
            int id = DiscoData.Instance.PlacedItems[instanceID].Item1;
            return GameBundle.Instance.AllInGameItems[id];
        }
    }
}
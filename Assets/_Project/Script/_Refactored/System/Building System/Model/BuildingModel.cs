using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using Framework.Context;
using Framework.Mvcs.Model;
using UnityEngine;

namespace System.Building_System.Model
{
    public class BuildingModel : BaseModel
    {
        public List<StoreItemSO> StoreItems;

        public override void Initialize(IContext context)
        {
            base.Initialize(context);
        
            StoreItems = new List<StoreItemSO>();
            foreach (var item in DiscoData.Instance.AllInGameItems)
                StoreItems.Add(item.Value);
        }

        public void AddPlacmeentItem(StoreItemSO itemSo, Transform sceneObject, Vector3 placedPosition, Quaternion placedRotation)
        {
            DiscoData.Instance.PlacedItems.Add(sceneObject.GetInstanceID(), new Tuple<int, Transform, Vector3, Quaternion>(itemSo.ID, sceneObject, placedPosition, placedRotation));
        }

        public void RemovePlacementItem(int ID)
        {
            DiscoData.Instance.PlacedItems.Remove(ID);
        }

        public Transform GetPlacedSceneObjectByID(int ID)
        {
            return DiscoData.Instance.PlacedItems[ID].Item2;
        }
    }
}
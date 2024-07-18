using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using BuildingSystem;
using BuildingSystem.SO;
using PropBehaviours;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Data
{
    // public class PlacementDataHandler
    // {
    //     private List<IPropUnit> propList;
    //     private Dictionary<Vector3Int, PlacementData> surfaceLayerPlacements;
    //
    //     private Dictionary<Vector3Int, PlacementData> propLayerPlacements;
    //
    //     public List<IPropUnit> GetPropList => propList;
    //
    //     public PlacementDataHandler()
    //     {
    //         propList = new List<IPropUnit>();
    //         surfaceLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
    //         propLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
    //     }
    //
    //     public bool ContainsKey(Vector3Int cellPos, ePlacementLayer layer)
    //     {
    //         if (cellPos.x < 0 ||
    //             cellPos.z < 0 ||
    //             cellPos.x >= DiscoData.Instance.MapData.CurrentMapSize.x ||
    //             cellPos.z >= DiscoData.Instance.MapData.CurrentMapSize.y)
    //             return true;
    //
    //         switch (layer)
    //         {
    //             case ePlacementLayer.BaseSurface:
    //                 return surfaceLayerPlacements.ContainsKey(cellPos);
    //             case ePlacementLayer.FloorProp:
    //             case ePlacementLayer.WallProp:
    //                 return propLayerPlacements.ContainsKey(cellPos);
    //             default:
    //                 Debug.Log($"Placement Layer Not Added : {layer.ToString()}");
    //                 return false;
    //         }
    //     }
    //
    //     public bool ContainsKey(Vector3Int cellPos, Vector2Int size, RotationData rotationData, ePlacementLayer layer)
    //     {
    //         var keys = CalculatePosition(cellPos, size,
    //             rotationData.direction);
    //
    //         foreach (var key in keys)
    //         {
    //             if (!ContainsKey(key, layer)) continue;
    //             return true;
    //         }
    //
    //         return false;
    //     }
    //
    //     private List<Vector3Int> CalculatePosition(Vector3Int cellPos, Vector2Int size, Direction direction)
    //     {
    //         var keys = new List<Vector3Int>();
    //         for (var x = 0; x < size.x; x++)
    //         for (var y = 0; y < size.y; y++)
    //             keys.Add(cellPos + GetKey(x, y, direction));
    //
    //         return keys;
    //     }
    //
    //     private Vector3Int GetKey(int x, int y, Direction direction)
    //     {
    //         var key = Vector3Int.zero;
    //         switch (direction)
    //         {
    //             case Direction.Up:
    //                 key.x = -x;
    //                 key.z = -y;
    //                 break;
    //             case Direction.Down:
    //                 key.x = x;
    //                 key.z = y;
    //                 break;
    //             case Direction.Left:
    //                 key.x = y;
    //                 key.z = -x;
    //                 break;
    //             case Direction.Right:
    //                 key.x = -y;
    //                 key.z = x;
    //                 break;
    //         }
    //
    //         return key;
    //     }
    //
    //     public void AddPlacementData(Vector3Int cellPos, PlacementData placementData, ePlacementLayer layer)
    //     {
    //         var keys = CalculatePosition(cellPos, placementData.Size,
    //             placementData.RotationData.direction);
    //
    //         foreach (var key in keys)
    //         {
    //             switch (layer)
    //             {
    //                 case ePlacementLayer.BaseSurface:
    //                     surfaceLayerPlacements.Add(key, placementData);
    //                     DiscoData.Instance.MapData.GetTileNodeByCellPos(cellPos).IsWalkable = true;
    //                     break;
    //                 case ePlacementLayer.FloorProp:
    //                 case ePlacementLayer.WallProp:
    //                     propLayerPlacements.Add(key, placementData);
    //                     DiscoData.Instance.MapData.GetTileNodeByCellPos(cellPos).IsWalkable = false;
    //                     break;
    //             }
    //             Debug.Log("Key : " + key);
    //         }
    //
    //         if (placementData.SceneObject.TryGetComponent(out IPropUnit prop))
    //         {
    //             propList.Add(prop);
    //             prop.Initialize(placementData.ID, cellPos, placementData.RotationData, layer);
    //         }
    //
    //         UpdateProps();
    //     }
    //
    //     public void RemovePlacementData(Vector3Int cellPos, ePlacementLayer layer)
    //     {
    //         var placementData = new PlacementData();
    //         switch (layer)
    //         {
    //             case ePlacementLayer.BaseSurface:
    //                 if (surfaceLayerPlacements.ContainsKey(cellPos))
    //                 {
    //                     placementData = surfaceLayerPlacements[cellPos];
    //                     surfaceLayerPlacements.Remove(cellPos);
    //                 }
    //
    //                 break;
    //             case ePlacementLayer.FloorProp:
    //             case ePlacementLayer.WallProp:
    //                 if (propLayerPlacements.ContainsKey(cellPos))
    //                 {
    //                     placementData = propLayerPlacements[cellPos];
    //                     propLayerPlacements.Remove(cellPos);
    //                 }
    //
    //                 break;
    //         }
    //
    //         var keys = CalculatePosition(placementData.PlacedCellPos, placementData.Size,
    //             placementData.RotationData.direction);
    //
    //         foreach (var key in keys)
    //         {
    //             switch (layer)
    //             {
    //                 case ePlacementLayer.BaseSurface:
    //                     surfaceLayerPlacements.Remove(key);
    //                     break;
    //                 case ePlacementLayer.FloorProp:
    //                 case ePlacementLayer.WallProp:
    //                     propLayerPlacements.Remove(key);
    //                     break;
    //             }
    //
    //             DiscoData.Instance.MapData.GetTileNodeByCellPos(cellPos).IsWalkable = true;
    //         }
    //
    //         DiscoData.Instance.inventory.AddItem(placementData.storeItemSo);
    //         var go = placementData.SceneObject;
    //         if (go.TryGetComponent(out IPropUnit prop)) propList.Remove(prop);
    //         Object.Destroy(go);
    //         UpdateProps();
    //     }
    //
    //     private void UpdateProps()
    //     {
    //         foreach (var prop in propList)
    //             if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
    //                 propUpdate.PropUpdate();
    //     }
    //
    //     public List<PlacementData> GetPlacementDatas(ePlacementLayer layer)
    //     {
    //         switch (layer)
    //         {
    //             case ePlacementLayer.BaseSurface:
    //                 return surfaceLayerPlacements.Values.ToList();
    //             case ePlacementLayer.FloorProp:
    //             case ePlacementLayer.WallProp:
    //                 return propLayerPlacements.Values.ToList();
    //             default:
    //                 Debug.LogError(layer.ToString() + " Is Missing");
    //                 return new List<PlacementData>();
    //         }
    //     }
    //
    //     public GameObject GetSceneObject(Vector3Int cellPos, ePlacementLayer layer)
    //     {
    //         switch (layer)
    //         {
    //             case ePlacementLayer.BaseSurface:
    //                 if (surfaceLayerPlacements.ContainsKey(cellPos))
    //                     return surfaceLayerPlacements[cellPos].SceneObject;
    //                 break;
    //             case ePlacementLayer.FloorProp:
    //             case ePlacementLayer.WallProp:
    //                 if (propLayerPlacements.ContainsKey(cellPos))
    //                     return propLayerPlacements[cellPos].SceneObject;
    //                 break;
    //             default:
    //                 Debug.LogError(layer.ToString() + " Is Missing");
    //                 return null;
    //         }
    //
    //         return null;
    //     }
    //     
    //
    //     #region Saving And Loading..
    //     public void LoadGameProps(GameData gameData)
    //     {
    //         surfaceLayerPlacements.Clear();
    //         
    //         Vector3Int test = -Vector3Int.one;
    //         foreach (var pair in gameData.savedPropLayerPlacements)
    //         {
    //             if (test != pair.Value.PlacedCellPosition)
    //             {
    //                 test = pair.Value.PlacedCellPosition;
    //                 var value = pair.Value;
    //                 var placementItemSo = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == value.PropID) as PlacementItemSO;
    //                 Debug.Log("Size : " + placementItemSo.Size);
    //             
    //                 InstantiateProp(placementItemSo, value.PlacedCellPosition, new RotationData(value.EularAngles, value.Direction));
    //             }
    //         }
    //     }
    //
    //     public void SaveGameProps(ref GameData gameData)
    //     {
    //         Debug.Log(propLayerPlacements.Count);
    //         gameData.savedPropLayerPlacements.Clear();
    //         
    //         Vector3Int test = -Vector3Int.one;
    //         foreach (var pair in propLayerPlacements)
    //         {
    //             if (test != pair.Value.PlacedCellPos)
    //             {
    //                 test = pair.Value.PlacedCellPos;
    //                 gameData.savedPropLayerPlacements.Add(pair.Key, new GameData.PlacementSaveData(pair.Value));
    //             }
    //         }
    //         
    //         Debug.Log(gameData.savedPropLayerPlacements.Count);
    //     }
    //
    //     private void InstantiateProp(PlacementItemSO placementItemso, Vector3Int cellPosition, RotationData rotationData) 
    //     {
    //         var createdObject = Object.Instantiate(placementItemso.Prefab,
    //             GridHandler.Instance.GetCellCenterWorld(cellPosition) +
    //             (placementItemso.PlacementLayer == ePlacementLayer.FloorProp
    //                 ? ConstantVariables.PropObjectOffset
    //                 : ConstantVariables.WallObjectOffset),
    //             rotationData.rotation);
    //
    //         createdObject.transform.SetParent(SceneGameObjectHandler.Instance.GetSurfaceHolderTransform);
    //
    //         PlacementData placementData = new PlacementData(placementItemso, cellPosition, createdObject, placementItemso.Size, rotationData);
    //         AddPlacementData(cellPosition, placementData, placementItemso.PlacementLayer);
    //     }
    //     
    //     #endregion
    //
    //     /// <summary>
    //     /// Only For Debug Purposes
    //     /// </summary>
    //     /// <param name="layer"></param>
    //     /// <returns></returns>
    //     public List<Vector3Int> GetUsedKeys(ePlacementLayer layer)
    //     {
    //         switch (layer)
    //         {
    //             case ePlacementLayer.BaseSurface:
    //                 return surfaceLayerPlacements.Keys.ToList();
    //             case ePlacementLayer.FloorProp:
    //             case ePlacementLayer.WallProp:
    //                 return propLayerPlacements.Keys.ToList();
    //             default:
    //                 Debug.LogError(layer.ToString() + " Is Missing");
    //                 return new List<Vector3Int>();
    //         }
    //     }
    // }

    public enum ePlacementLayer
    {
        BaseSurface, // General BaseSurface Placement
        FloorProp, // Objects placed on the Floor
        WallProp, // Objects placed ont the Wall
        Null
    }

    public class PlacementDataHandler
    {
        public class NewPlacementData
        {
            public int ID;
            public Vector3Int CellPositionToPlace;
            public PlacementItemSO PlacementItemToPlace;
            public GameObject SceneObjectToPlaced;
            public Vector2Int Size;
            public RotationData RotationToSet;
            
            public NewPlacementData(PlacementItemSO storeItemSo, Vector3Int placedCellPos, GameObject createdObject, RotationData rotationData)
            {
                ID = storeItemSo.ID;
                CellPositionToPlace = placedCellPos;
                PlacementItemToPlace = storeItemSo;
                SceneObjectToPlaced = createdObject;
                RotationToSet = rotationData;
                Size = storeItemSo.Size;
            }

            public NewPlacementData()
            {
                ID = -1;
                CellPositionToPlace = -Vector3Int.one;
                PlacementItemToPlace = null;
                SceneObjectToPlaced = null;
                RotationToSet = RotationData.Default;
                Size = Vector2Int.one;
            }
        }

        public class TupleTest
        {
            private Tuple<Vector3Int, NewPlacementData, ePlacementLayer> Tuple;

            public TupleTest()
            {
                Tuple = new Tuple<Vector3Int, NewPlacementData, ePlacementLayer>(Vector3Int.zero, new NewPlacementData(), ePlacementLayer.Null);
            }
        }

        private List<IPropUnit> propList;

        // Data - PlacedPosition
        private Dictionary<Vector3Int, NewPlacementData> allPropPlacementDatas;
        private Dictionary<Vector3Int, NewPlacementData> allBaseSurfacePlacementDatas;
        
        private Dictionary<TupleTest, Vector3Int> tupleTest;

        public PlacementDataHandler()
        {
            propList = new List<IPropUnit>();
            allPropPlacementDatas = new Dictionary<Vector3Int, NewPlacementData>();
            allBaseSurfacePlacementDatas = new Dictionary<Vector3Int, NewPlacementData>();

            tupleTest = new Dictionary<TupleTest, Vector3Int>();
        }

        public bool ContainsKey(Vector3Int cellPos, ePlacementLayer layer)
        {
            if (cellPos.x < 0 ||
                cellPos.z < 0 ||
                cellPos.x >= MapData.CurrentMapSize.x ||
                cellPos.z >= MapData.CurrentMapSize.y)
                return true;

            return MapData.GetFloorGridAssignmentByCellPos(cellPos).assignedObjectID == -1 ? false : true;
        }
        
        public bool ContainsKey(Vector3Int cellPos, Vector2Int size, RotationData rotationData, ePlacementLayer layer)
        {
            var keys = CalculatePosition(cellPos, size,
                rotationData.direction);
        
            foreach (var key in keys)
            {
                if (!ContainsKey(key, layer)) continue;
                return true;
            }
        
            return false;
        }

        public void AddPlacement(Vector3Int cellPos, NewPlacementData placementData)
        {
            var keys = CalculatePosition(cellPos, placementData.Size,
                placementData.RotationToSet.direction);

            foreach (var key in keys)
            {
                AssignIDToFloorGridData(placementData.PlacementItemToPlace.PlacementLayer, key, placementData.ID);
            }

            switch (placementData.PlacementItemToPlace.PlacementLayer)
            {
                case ePlacementLayer.BaseSurface:
                    allBaseSurfacePlacementDatas.Add(cellPos,placementData);
                    break;
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.WallProp:
                    allPropPlacementDatas.Add(cellPos, placementData);
                    break;
            }
            
            if (placementData.SceneObjectToPlaced.TryGetComponent(out IPropUnit prop))
            {
                propList.Add(prop);
                prop.Initialize(placementData.ID, cellPos, placementData.RotationToSet, placementData.PlacementItemToPlace.PlacementLayer);
            }
            
            UpdateProps();
        }

        public void RemovePlacement(Vector3Int cellPos, ePlacementLayer moveFromLayer)
        {
            NewPlacementData data = null;
            
            switch (moveFromLayer)
            {
                case ePlacementLayer.BaseSurface:
                    data = allBaseSurfacePlacementDatas[cellPos];
                    allBaseSurfacePlacementDatas.Remove(cellPos);
                    break;
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.WallProp:
                    data = allPropPlacementDatas[cellPos];
                    allPropPlacementDatas.Remove(cellPos);
                    break;
            }
            
            if(data == null) Debug.LogError("Data was NULL means cellpos not exist : " + cellPos);
            
            var keys = CalculatePosition(data.CellPositionToPlace, data.Size,
                data.RotationToSet.direction);

            foreach (var key in keys)
            {
                AssignIDToFloorGridData(data.PlacementItemToPlace.PlacementLayer, key, -1);
            }
            
            DiscoData.Instance.inventory.AddItem(data.PlacementItemToPlace);
            
            var objectToRemove = data.SceneObjectToPlaced;
            if (objectToRemove.TryGetComponent(out IPropUnit prop)) propList.Remove(prop);
            Object.Destroy(objectToRemove);
            UpdateProps();
        }
        
        private void UpdateProps()
        {
            foreach (var prop in propList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();
        }
        
        private List<Vector3Int> CalculatePosition(Vector3Int cellPos, Vector2Int size, Direction direction)
        {
            var keys = new List<Vector3Int>();
            for (var x = 0; x < size.x; x++)
            for (var y = 0; y < size.y; y++)
                keys.Add(cellPos + GetKey(x, y, direction));

            return keys;
        }

        private Vector3Int GetKey(int x, int y, Direction direction)
        {
            var key = Vector3Int.zero;
            switch (direction)
            {
                case Direction.Up:
                    key.x = -x;
                    key.z = -y;
                    break;
                case Direction.Down:
                    key.x = x;
                    key.z = y;
                    break;
                case Direction.Left:
                    key.x = y;
                    key.z = -x;
                    break;
                case Direction.Right:
                    key.x = -y;
                    key.z = x;
                    break;
            }

            return key;
        }
        
        private void AssignIDToFloorGridData(ePlacementLayer layer, Vector3Int key, int newID)
        {
            bool walkable = newID == -1 ? true : false;
            switch (layer)
            {
                case ePlacementLayer.BaseSurface:
                    MapData.GetFloorGridAssignmentByCellPos(key).AssignNewID(eFloorGridAssignmentType.Surface, newID);
                    break;
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.WallProp:
                    MapData.GetFloorGridAssignmentByCellPos(key).AssignNewID(eFloorGridAssignmentType.Object, newID);
                    MapData.GetTileNodeByCellPos(key).IsWalkable = walkable;
                    break;
            }
        }
        
        #region Saving And Loading..
        
        public void LoadGameProps(GameData gameData)
        {
            allPropPlacementDatas.Clear();
            allBaseSurfacePlacementDatas.Clear();
            
            Vector3Int test = -Vector3Int.one;
            foreach (var pair in gameData.savedAllPropLayerPlacements)
            {
                if (test != pair.Value.PlacedCellPosition)
                {
                    test = pair.Value.PlacedCellPosition;
                    var value = pair.Value;
                    var placementItemSo = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == value.PropID) as PlacementItemSO;
                    InstantiateProp(placementItemSo, value.PlacedCellPosition, new RotationData(value.EularAngles, value.Direction));
                }
            }
            
            foreach (var pair in gameData.savedAllSurfaceLayerPlacements)
            {
                if (test != pair.Value.PlacedCellPosition)
                {
                    test = pair.Value.PlacedCellPosition;
                    var value = pair.Value;
                    var placementItemSo = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == value.PropID) as PlacementItemSO;
                    InstantiateProp(placementItemSo, value.PlacedCellPosition, new RotationData(value.EularAngles, value.Direction));
                }
            }
        }
        
        public void SaveGameProps(ref GameData gameData)
        {
            gameData.savedAllPropLayerPlacements.Clear();
            gameData.savedAllSurfaceLayerPlacements.Clear();
            
            foreach (var pair in allPropPlacementDatas)
                gameData.savedAllPropLayerPlacements.Add(pair.Key, new GameData.PlacementSaveData(pair.Value));
            
            foreach (var pair in allBaseSurfacePlacementDatas)
                gameData.savedAllSurfaceLayerPlacements.Add(pair.Key, new GameData.PlacementSaveData(pair.Value));
            
            Debug.Log(gameData.savedAllPropLayerPlacements.Count);
        }
        
        private void InstantiateProp(PlacementItemSO placementItemso, Vector3Int cellPosition, RotationData rotationData)
        {
            Vector3 offset = (placementItemso.PlacementLayer == ePlacementLayer.FloorProp || placementItemso.PlacementLayer == ePlacementLayer.BaseSurface
                ? ConstantVariables.PropObjectOffset
                : ConstantVariables.WallObjectOffset);
            
            var createdObject = Object.Instantiate(placementItemso.Prefab, GridHandler.Instance.GetCellCenterWorld(cellPosition) + offset, rotationData.rotation);
        
            createdObject.transform.SetParent(SceneGameObjectHandler.Instance.GetSurfaceHolderTransform);
        
            NewPlacementData placementData = new NewPlacementData(placementItemso, cellPosition, createdObject, rotationData);
            AddPlacement(cellPosition, placementData);
        }
        #endregion
            
        public List<NewPlacementData> GetPlacementDatas(ePlacementLayer layer)
        {
            return allPropPlacementDatas.Values.ToList();
        }
        
        /// <summary>
        /// Only For Debug Purposes
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public List<Vector3Int> GetUsedKeys(ePlacementLayer layer)
        {
            return allPropPlacementDatas.Keys.ToList();
        }

        public List<IPropUnit> GetPropList => propList;

        private MapData MapData => DiscoData.Instance.MapData;
    }
}
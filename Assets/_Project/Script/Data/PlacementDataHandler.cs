using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using BuildingSystem;
using BuildingSystem.Builders;
using BuildingSystem.SO;
using PropBehaviours;
using SerializableTypes;
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
        public class PlacementData
        {
            public int ID;
            public Vector3Int PlacedCellPosition;
            public PlacementItemSO PlacedPlacementItemSo;
            public GameObject PlacedSceneObject;
            public RotationData SettedRotationData;
            
            public PlacementData(PlacementItemSO storeItemSo, Vector3Int placedPlacedCellPos, GameObject createdObject, RotationData settedRotationData)
            {
                ID = storeItemSo.ID;
                PlacedCellPosition = placedPlacedCellPos;
                PlacedPlacementItemSo = storeItemSo;
                PlacedSceneObject = createdObject;
                SettedRotationData = settedRotationData;
            }
        }

        private List<IPropUnit> propList;
        //                  placement data     worldPositions      layer
        private HashSet<Tuple<PlacementData, List<Vector3Int>, ePlacementLayer>> AllPlacedObjects;
       
        public PlacementDataHandler()
        {
            propList = new List<IPropUnit>();
            AllPlacedObjects = new HashSet<Tuple<PlacementData, List<Vector3Int>, ePlacementLayer>>();
        }

        public bool ContainsKey(Vector3Int cellPos, ePlacementLayer layer)
        {
            if (cellPos.x < 0 ||
                cellPos.z < 0 ||
                cellPos.x >= MapData.CurrentMapSize.x ||
                cellPos.z >= MapData.CurrentMapSize.y)
                return true;

            foreach (var usedKeys in AllPlacedObjects)
            {
                if(usedKeys.Item3 != layer) continue;

                foreach (var pos in usedKeys.Item2)
                    if (pos == cellPos) return pos == cellPos ? true : false;
                
            }
            
            return false;
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

        public void AddPlacement(Vector3Int cellPos, PlacementData placementData)
        {
            var keys = CalculatePosition(cellPos, placementData.PlacedPlacementItemSo.Size,
                placementData.SettedRotationData.direction);
            
            AllPlacedObjects.Add(new Tuple<PlacementData, List<Vector3Int>, ePlacementLayer>(placementData, new List<Vector3Int>(), placementData.PlacedPlacementItemSo.PlacementLayer));
            
            var found = AllPlacedObjects.FirstOrDefault(x => x.Item1.PlacedCellPosition == cellPos && x.Item3 == placementData.PlacedPlacementItemSo.PlacementLayer);

            foreach (var key in keys)
            {
                found.Item2.Add(key);
                MapData.PathFinderNodes[key.x, key.z].IsWalkable =
                    placementData.PlacedPlacementItemSo.PlacementLayer == ePlacementLayer.BaseSurface ? true : false;
            }
            
            if (placementData.PlacedSceneObject.TryGetComponent(out IPropUnit prop))
            {
                propList.Add(prop);
                prop.Initialize(placementData.ID, cellPos, placementData.SettedRotationData, placementData.PlacedPlacementItemSo.PlacementLayer);
            }
            
            UpdateProps();
        }

        public void RemovePlacement(Vector3Int cellPos, ePlacementLayer moveFromLayer)
        {
            var data = AllPlacedObjects.FirstOrDefault(x => x.Item1.PlacedCellPosition == cellPos && x.Item3 == moveFromLayer);
            
            if(data == null) Debug.LogError("Data was NULL means cellpos not exist : " + cellPos);
            
            var keys = CalculatePosition(data.Item1.PlacedCellPosition, data.Item1.PlacedPlacementItemSo.Size,
                data.Item1.SettedRotationData.direction);

            foreach (var key in keys)
                MapData.GetTileNodeByCellPos(key).IsWalkable = true;

            DiscoData.Instance.inventory.AddItem(data.Item1.PlacedPlacementItemSo);
            
            AllPlacedObjects.Remove(data);

            var objectToRemove = data.Item1.PlacedSceneObject;
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
        
        #region Saving And Loading..
        
        public void LoadGameProps(GameData gameData)
        {
            AllPlacedObjects.Clear();

            PlacementBuilder builder = new PlacementBuilder();

            foreach (var savedData in gameData.SavedPlacementDatas)
            {
                var placementItemSo = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == savedData.PropID) as PlacementItemSO;
                
                RotationData rotationData = new RotationData(savedData.EularAngles, savedData.Direction);

                var createdObject = builder.InstantiateProp(placementItemSo, savedData.PlacedCellPosition, rotationData);
                PlacementData placementData = new PlacementData(placementItemSo, savedData.PlacedCellPosition, createdObject, rotationData);
                
                AddPlacement(savedData.PlacedCellPosition, placementData);
            }
        }
        
        public void SaveGameProps(ref GameData gameData)
        {
            gameData.SavedPlacementDatas.Clear();

            foreach (var placedObject in AllPlacedObjects)
                gameData.SavedPlacementDatas.Add(new GameData.PlacementSaveData(placedObject.Item1));
        }
        
        #endregion
            
        public List<PlacementData> GetPlacementDatas(ePlacementLayer layer)
        {
            List<PlacementData> output = new List<PlacementData>();
            foreach (var item in AllPlacedObjects)
                output.Add(item.Item1);
            
            return output;
        }

        public Transform GetPlacementObjectByCellPos(Vector3Int cellPosition)
        {
            foreach (var tuple in AllPlacedObjects)
                foreach (var keys in tuple.Item2)
                    if (keys == cellPosition)
                        return tuple.Item1.PlacedSceneObject.transform;
            
            return null;
        }
        
        /// <summary>
        /// Only For Debug Purposes
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public List<Vector3Int> GetUsedKeys(ePlacementLayer layer)
        {
            List<Vector3Int> output = new List<Vector3Int>();
            foreach (var key in AllPlacedObjects)
                foreach (var pos in key.Item2)
                    output.Add(pos);
            
            return output;
        }

        public List<IPropUnit> GetPropList => propList;

        private MapData MapData => DiscoData.Instance.MapData;
    }
}
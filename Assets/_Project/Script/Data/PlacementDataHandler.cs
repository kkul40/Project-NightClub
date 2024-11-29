using System;
using System.Collections.Generic;
using System.Linq;
using Disco_Building;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data
{
    public class PlacementDataHandler
    {
        public class PlacementData
        {
            public int ID;
            public Vector3Int PlacedCellPosition;
            public PlacementItemSO PlacedPlacementItemSo;
            public GameObject PlacedSceneObject;
            public RotationData SettedRotationData;

            public PlacementData(PlacementItemSO storeItemSo, Vector3Int placedPlacedCellPos, GameObject createdObject,
                RotationData settedRotationData)
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

        public static event Action OnPropUpdate;
        public static event Action OnPropRemoved;
        public static event Action OnPropPlaced;
        public static event Action<List<Vector3Int>> OnPlacedPositions;

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

            if (layer != ePlacementLayer.BaseSurface)
            {
                for (int i = 0; i < ConstantVariables.DoorHeight; i++)
                {
                    if (cellPos == MapData.EnterencePosition().WorldPosToCellPos(eGridType.PlacementGrid).Add(y:i))
                        return true;
                }
            }

            foreach (var usedKeys in AllPlacedObjects)
            {
                if (usedKeys.Item3 != layer) continue;

                foreach (var pos in usedKeys.Item2)
                    if (pos == cellPos)
                        return pos == cellPos ? true : false;
            }

            return false;
        }

        public bool ContainsKeyOnWall(Vector3Int cellPos, int height)
        {
            for (int i = 0; i < height; i++)
            {
                if (ContainsKey(cellPos.Add(y:i), ePlacementLayer.WallProp)) return true;
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

            AllPlacedObjects.Add(new Tuple<PlacementData, List<Vector3Int>, ePlacementLayer>(placementData,
                new List<Vector3Int>(), placementData.PlacedPlacementItemSo.PlacementLayer));

            var addedData = AllPlacedObjects.FirstOrDefault(x => x.Item1.PlacedCellPosition == cellPos && x.Item3 == placementData.PlacedPlacementItemSo.PlacementLayer);
            
            foreach (var key in keys)
                addedData.Item2.Add(key);
            
            UpdatePathFinder(addedData);
            
            OnPropPlaced?.Invoke();
            OnPlacedPositions?.Invoke(keys);
            
            AddedObjectHandler(cellPos, placementData);
            UpdateProps();
        }

        public void RemovePlacement(Vector3Int cellPos, ePlacementLayer moveFromLayer, bool addToInventory = true, bool removeImidietly = false)
        {
            var dataToRemove = GetPlacementDataByCellPos(cellPos, moveFromLayer);

            if (dataToRemove == null) Debug.LogError("Data was NULL means cellpos not exist : " + cellPos);

            // var keys = CalculatePosition(dataToRemove.Item1.PlacedCellPosition, dataToRemove.Item1.PlacedPlacementItemSo.Size,
            //     dataToRemove.Item1.SettedRotationData.direction);
            
            if(addToInventory)
                DiscoData.Instance.inventory.AddItem(dataToRemove.Item1.PlacedPlacementItemSo);
            
            AllPlacedObjects.Remove(dataToRemove);
            
            UpdatePathFinderNearRemovedObject(cellPos, dataToRemove);
            
            RemoveObjectHandler(dataToRemove.Item1.PlacedSceneObject, removeImidietly);
            OnPropRemoved?.Invoke();
            UpdateProps();
        }

        private void UpdatePathFinderNearRemovedObject(Vector3Int cellPos,  Tuple<PlacementData, List<Vector3Int>, ePlacementLayer> data)
        {
            if (data.Item3 != ePlacementLayer.FloorProp) return;
            
            foreach (var key in data.Item2)
                MapData.Path.UpdatePathFinderNode(key, data.Item1.PlacedPlacementItemSo.IsBig, true);
            
            List<Vector3Int> updadatableKeys = new List<Vector3Int>();
            foreach (var worldPos in data.Item2)
                foreach (var key in worldPos.GetNearByKeys())
                    updadatableKeys.Add(key);
            
            foreach (var key in updadatableKeys)
            {
                var tuple = GetPlacementDataByCellPos(key, data.Item3);
                if(tuple == null) continue;

                foreach (var worldKey in tuple.Item2)
                    MapData.Path.UpdatePathFinderNode(worldKey, tuple.Item1.PlacedPlacementItemSo.IsBig, false);
            }
        }
        
        private void UpdatePathFinder(Tuple<PlacementData, List<Vector3Int>, ePlacementLayer> tuple)
        {
            if (tuple.Item3 != ePlacementLayer.FloorProp) return;

            foreach (var key in tuple.Item2)
                MapData.Path.UpdatePathFinderNode(key, tuple.Item1.PlacedPlacementItemSo.IsBig , false);
        }
        
        private void AddedObjectHandler(Vector3Int cellPos, PlacementData placementData)
        {
            IPropUnit propUnit = null;
            
            if (placementData.PlacedSceneObject.TryGetComponent(out IPropUnit prop))
                propUnit = prop;
            else
                propUnit = placementData.PlacedSceneObject.AddComponent<IPropUnit>();
            
            propList.Add(prop);
            prop.Initialize(placementData.ID, cellPos, placementData.SettedRotationData, placementData.PlacedPlacementItemSo.PlacementLayer);

            if (placementData.PlacedSceneObject.TryGetComponent(out IPropUpdate propUpdate)) propUpdate.OnPropPlaced();
        }


        private void RemoveObjectHandler(GameObject sceneObject, bool removeImmidietly)
        {
            var objectToRemove = sceneObject;
            if (objectToRemove.TryGetComponent(out IPropUpdate propUpdate)) propUpdate.OnPropRemoved();

            if (objectToRemove.TryGetComponent(out IPropUnit prop)) propList.Remove(prop);

            if (removeImmidietly)
                Object.Destroy(objectToRemove);
            else
                objectToRemove.AnimatedRemoval(() => Object.Destroy(objectToRemove));
        }

        private void UpdateProps()
        {
            foreach (var prop in propList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();

            OnPropUpdate?.Invoke();
        }
        

        /// <summary>
        /// Returns All Keys That Object Will Use
        /// </summary>
        /// <param name="cellPos"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
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

            foreach (var savedData in gameData.SavedPlacementDatas)
            {
                var placementItemSo = DiscoData.Instance.FindAItemByID(savedData.PropID) as PlacementItemSO;

                if (placementItemSo == null)
                {
                    Debug.LogError("Could Not Find Object : " + savedData.PropID);
                    continue;
                }

                var rotationData = new RotationData(savedData.EularAngles, savedData.Direction);

                var createdObject = InstantiateProp(placementItemSo, savedData.PlacedCellPosition, rotationData);
                var placementData = new PlacementData(placementItemSo, savedData.PlacedCellPosition, createdObject,
                    rotationData);

                AddPlacement(savedData.PlacedCellPosition, placementData);
            }
        }

        private GameObject InstantiateProp(PlacementItemSO placementItemso, Vector3Int cellPosition,
            RotationData rotationData)
        {
            var createdObject = Object.Instantiate(placementItemso.Prefab, cellPosition.CellCenterPosition(eGridType.PlacementGrid), rotationData.rotation);
            createdObject.AnimatedPlacement(cellPosition.CellCenterPosition(eGridType.PlacementGrid));
            
            createdObject.transform.SetParent(SceneGameObjectHandler.Instance.GetHolderByLayer(placementItemso.PlacementLayer));

            return createdObject;
        }

        public void SaveGameProps(ref GameData gameData)
        {
            gameData.SavedPlacementDatas.Clear();

            foreach (var placedObject in AllPlacedObjects)
                gameData.SavedPlacementDatas.Add(placedObject.Item1.ConvertToPlacementSaveData());
        }

        #endregion

        public List<PlacementData> GetPlacementDatas()
        {
            var output = new List<PlacementData>();
            foreach (var item in AllPlacedObjects)
                output.Add(item.Item1);

            return output;
        }

        public Tuple<PlacementData, List<Vector3Int>, ePlacementLayer> GetPlacementDataByCellPos(Vector3Int cellPosition, ePlacementLayer layer)
        {
            Tuple<PlacementData, List<Vector3Int>, ePlacementLayer> output = null;
            
            foreach (var item in AllPlacedObjects)
            {
                if(item.Item3 != layer) continue;
                
                if (cellPosition == item.Item1.PlacedCellPosition)
                {
                    output = item;
                    break;
                }

                foreach (var key in item.Item2)
                {
                    if (cellPosition == key)
                    {
                        output = item;
                        break;
                    }
                }
            }
            
            return output;
        }

        public Transform GetPlacementObjectByCellPos(Vector3Int cellPosition, ePlacementLayer layer)
        {
            foreach (var tuple in AllPlacedObjects)
            foreach (var keys in tuple.Item2)
            {
                if (tuple.Item3 != layer) continue;

                if (keys == cellPosition)
                    return tuple.Item1.PlacedSceneObject.transform;
            }

            return null;
        }
        
        // public bool IsWorldPositionClearOfAnyPlacement(Vector3 vector)
        // {
        //     foreach (var value in Enum.GetValues(typeof(ePlacementLayer).GetType()))
        //     {
        //         if((ePlacementLayer)value == ePlacementLayer.WallProp) continue;
        //         
        //         ContainsKey()
        //     }
        // }

        /// <summary>
        /// Only For Debug Purposes
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public List<Vector3Int> GetUsedKeys(ePlacementLayer layer)
        {
            var output = new List<Vector3Int>();
            foreach (var key in AllPlacedObjects)
            foreach (var pos in key.Item2)
                output.Add(pos);

            return output;
        }

        public List<IPropUnit> GetPropList => propList;

        private MapData MapData => DiscoData.Instance.MapData;

        public List<T> GetPropsByType<T>()
        {
            var output = new List<T>();

            foreach (var tuple in AllPlacedObjects)
            {
                if (tuple.Item1.PlacedSceneObject.TryGetComponent(out T t))
                {
                    output.Add(t);
                }
            }

            return output;
        }
    }
}


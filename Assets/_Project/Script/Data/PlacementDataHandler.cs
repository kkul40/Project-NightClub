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
                if (usedKeys.Item3 != layer) continue;

                foreach (var pos in usedKeys.Item2)
                    if (pos == cellPos)
                        return pos == cellPos ? true : false;
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

            var found = AllPlacedObjects.FirstOrDefault(x =>
                x.Item1.PlacedCellPosition == cellPos && x.Item3 == placementData.PlacedPlacementItemSo.PlacementLayer);


            var isWalkable = placementData.PlacedPlacementItemSo.PlacementLayer == ePlacementLayer.FloorProp
                ? false
                : true;
            foreach (var key in keys)
            {
                found.Item2.Add(key);
                MapData.SetPathfinderNode(key.x, key.z, isWalkable: false);
            }

            if (placementData.PlacedSceneObject.TryGetComponent(out IPropUnit prop))
            {
                propList.Add(prop);
                prop.Initialize(placementData.ID, cellPos, placementData.SettedRotationData,
                    placementData.PlacedPlacementItemSo.PlacementLayer);
            }

            UpdateProps();
        }

        public void RemovePlacement(Vector3Int cellPos, ePlacementLayer moveFromLayer)
        {
            var data = AllPlacedObjects.FirstOrDefault(x =>
                x.Item1.PlacedCellPosition == cellPos && x.Item3 == moveFromLayer);

            if (data == null) Debug.LogError("Data was NULL means cellpos not exist : " + cellPos);

            var keys = CalculatePosition(data.Item1.PlacedCellPosition, data.Item1.PlacedPlacementItemSo.Size,
                data.Item1.SettedRotationData.direction);

            foreach (var key in keys)
                MapData.SetPathfinderNode(key.x, key.z, isWalkable: true);

            DiscoData.Instance.inventory.AddItem(data.Item1.PlacedPlacementItemSo);

            AllPlacedObjects.Remove(data);

            var objectToRemove = data.Item1.PlacedSceneObject;
            if (objectToRemove.TryGetComponent(out IPropUnit prop))
            {
                propList.Remove(prop);
            }

            Object.Destroy(objectToRemove);
            UpdateProps();
        }

        private void UpdateProps()
        {
            foreach (var prop in propList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();
            
            OnPropUpdate?.Invoke();
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
            //Sahneden Zaten Siliniyor

            foreach (var savedData in gameData.SavedPlacementDatas)
            {
                var placementItemSo = DiscoData.Instance.FindItemByID(savedData.PropID) as PlacementItemSO;

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
            Vector3 offset = new Vector3().BuildingOffset(placementItemso.PlacementLayer);
            var createdObject = Object.Instantiate(placementItemso.Prefab,
                GridHandler.Instance.GetCellCenterWorld(cellPosition, eGridType.PlacementGrid) + offset, rotationData.rotation);
            createdObject.transform.SetParent(
                SceneGameObjectHandler.Instance.GetHolderByLayer(placementItemso.PlacementLayer));

            return createdObject;
        }

        public void SaveGameProps(ref GameData gameData)
        {
            gameData.SavedPlacementDatas.Clear();

            foreach (var placedObject in AllPlacedObjects)
                gameData.SavedPlacementDatas.Add(new GameData.PlacementSaveData(placedObject.Item1));
        }

        #endregion

        public List<PlacementData> GetPlacementDatas()
        {
            var output = new List<PlacementData>();
            foreach (var item in AllPlacedObjects)
                output.Add(item.Item1);

            return output;
        }

        public Transform GetPlacementObjectByCellPos(Vector3Int cellPosition, ePlacementLayer layer)
        {
            foreach (var tuple in AllPlacedObjects)
            foreach (var keys in tuple.Item2)
            {
                if(tuple.Item3 != layer) continue;
                
                if (keys == cellPosition)
                    return tuple.Item1.PlacedSceneObject.transform;
            }

            return null;
        }

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
    }

    public enum ePlacementLayer
    {
        BaseSurface, // General BaseSurface Placement
        FloorProp, // Objects placed on the Floor
        WallProp, // Objects placed ont the Wall
        Null
    }
}
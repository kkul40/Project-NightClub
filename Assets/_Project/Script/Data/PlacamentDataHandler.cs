using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data
{
    public class PlacamentDataHandler
    {
        // TODO Yerde 3 farkli layer olacak Floor, Dance, PlacamentLayer Ve Birbirlerinin uzerine yerlestirilebilecekler
        private Dictionary<Vector3Int, PlacementData> placementDatas;

        public PlacamentDataHandler()
        {
            placementDatas = new Dictionary<Vector3Int, PlacementData>();
            GetPropList = new List<Prop>();
        }

        public List<Prop> GetPropList { get; }

        public bool ContainsKey(Vector3Int cellPos, PlacementMethodType placementMethodType)
        {
            var border = GameData.Instance.MapGenerator.MapSize;

            if (cellPos.x < 0 || cellPos.z < 0) return true;

            if (cellPos.x >= border.x || cellPos.z >= border.y)
                return true; // TODO Kapinin kordinatlari bunlar ve baska bir yere tasi

            if (cellPos.x == 4 && cellPos.z == 0) return true;

            if (placementDatas.TryGetValue(cellPos, out var data))
                if (data.placableItemData.PlacementMethodType == placementMethodType)
                    return true;
            return false;
        }

        public bool ContainsKey(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
            PlacementMethodType placementMethodType)
        {
            var keys = new List<Vector3Int>();

            keys = GeneratePlacableKeys(cellPos, objectSize, direction);

            foreach (var key in keys)
                if (ContainsKey(cellPos, placementMethodType))
                    return true;
            return false;
        }

        public void RemovePlacementData(Vector3Int cellPos, PlacementMethodType placementMethodType)
        {
            if (!ContainsKey(cellPos, placementMethodType)) return;

            var go = placementDatas[cellPos].SceneObject;

            TryRemoveProp(go);
            Object.Destroy(go);
            UpdateProps();

            var value = placementDatas[cellPos];

            var keys = new List<Vector3Int>();
            foreach (var key in placementDatas.Keys)
                if (Equals(placementDatas[key], value))
                    keys.Add(key);
            foreach (var key in keys) placementDatas.Remove(key);
        }

        public void AddPlacementData(Vector3Int cellPos, PlacementData placementData)
        {
            if (ContainsKey(cellPos, placementData.placableItemData.PlacementMethodType)) return;

            var objectSize = placementData.placableItemData.Size;
            var direction = placementData.RotationData.direction;
            var keys = new List<Vector3Int>();

            keys = GeneratePlacableKeys(cellPos, objectSize, direction);

            var keysAreValid = true;
            foreach (var key in keys)
            {
                keysAreValid = !ContainsKey(key, placementData.placableItemData.PlacementMethodType);

                if (!keysAreValid) return;
            }

            if (keysAreValid)
            {
                foreach (var key in keys) placementDatas.Add(key, placementData);
                TryAddProp(placementData.SceneObject);
                UpdateProps();
            }
        }

        private List<Vector3Int> GeneratePlacableKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction)
        {
            var keys = new List<Vector3Int>();
            keys.Add(cellPos);
            CheckHorizontalKeys(cellPos, objectSize, direction, ref keys);
            return keys;
        }

        private void CheckHorizontalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
            ref List<Vector3Int> keys)
        {
            CheckVerticalKeys(cellPos, objectSize, direction, ref keys);

            for (var i = 1; i < objectSize.x; i++)
            {
                var newCellPos = -Vector3Int.one;
                switch (direction)
                {
                    case Direction.Down:
                        newCellPos = cellPos + Vector3Int.left * i;
                        break;
                    case Direction.Up:
                        newCellPos = cellPos + Vector3Int.right * i;
                        break;
                    case Direction.Left:
                        newCellPos = cellPos + Vector3Int.forward * i;
                        break;
                    case Direction.Right:
                        newCellPos = cellPos + Vector3Int.back * i;
                        break;
                }

                if (newCellPos != -Vector3Int.one)
                {
                    keys.Add(newCellPos);
                    CheckVerticalKeys(newCellPos, objectSize, direction, ref keys);
                }
            }
        }

        private void CheckVerticalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
            ref List<Vector3Int> keys)
        {
            for (var i = 1; i < objectSize.y; i++)
            {
                var newCellPos = -Vector3Int.one;
                switch (direction)
                {
                    case Direction.Down:
                        newCellPos = cellPos + Vector3Int.forward * i;
                        break;
                    case Direction.Up:
                        newCellPos = cellPos + Vector3Int.back * i;
                        break;
                    case Direction.Left:
                        newCellPos = cellPos + Vector3Int.right * i;
                        break;
                    case Direction.Right:
                        newCellPos = cellPos + Vector3Int.left * i;
                        break;
                }

                if (newCellPos != -Vector3Int.one) keys.Add(newCellPos);
            }
        }

        private void TryAddProp(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out Prop prop)) GetPropList.Add(prop);
        }

        private void TryRemoveProp(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out Prop prop)) GetPropList.Remove(prop);
        }

        public GameObject GetPlacedObjectByCellPosition(Vector3Int cellPos)
        {
            if (placementDatas.TryGetValue(cellPos, out var value)) return value.SceneObject;

            return null;
        }

        public List<PlacementData> GetPlacementData()
        {
            return placementDatas.Values.ToList();
        }

        private void UpdateProps()
        {
            foreach (var prop in GetPropList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();
        }
    }
}
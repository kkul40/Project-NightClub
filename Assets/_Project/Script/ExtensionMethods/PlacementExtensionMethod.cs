using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using DiscoSystem;
using UnityEngine;

namespace ExtensionMethods
{
    public static class PlacementExtensionMethod
    {
        public static List<Vector3Int> GetNearByKeys(this Vector3Int vector)
        {
            List<Vector3Int> keys = new List<Vector3Int>();
            
            keys.Add(vector + new Vector3Int(-1,0,1));
            keys.Add(vector + new Vector3Int(0,0,1));
            keys.Add(vector + new Vector3Int(1,0,1));
            
            keys.Add(vector + new Vector3Int(-1,0,0));
            keys.Add(vector + new Vector3Int(1,0,0));
            
            keys.Add(vector + new Vector3Int(-1,0,-1));
            keys.Add(vector + new Vector3Int(0,0,-1));
            keys.Add(vector + new Vector3Int(1,0,-1));
            
            return keys;
        }

        public static Vector3 GetPlacementCenter(this Vector3Int cellPosition, Vector2 objectSize, Direction direction)
        {
            if (objectSize == Vector2.one) 
                return cellPosition.CellCenterPosition(eGridType.PlacementGrid);

            Vector3 centerPosition = cellPosition.CellCenterPosition(eGridType.PlacementGrid);
            Vector2Int halfSize = new Vector2Int((int)objectSize.x / 2, (int)objectSize.y / 2);
            Vector3 output = centerPosition;
            
            switch (direction)
            {
                case Direction.Down:
                    output += new Vector3(halfSize.x , 0, halfSize.y);
                    break;
                case Direction.Left:
                    output += new Vector3(halfSize.y , 0, -halfSize.x);
                    break;
                case Direction.Up:
                    output += new Vector3(-halfSize.x, 0, -halfSize.y);
                    break;
                case Direction.Right:
                    output += new Vector3(-halfSize.y, 0, halfSize.x);
                    break;
            }
            
            return output;
        }

        public static void AnimatedPlacement(this GameObject gameObject, Vector3 placedPosition)
        {
            gameObject.transform.position = placedPosition.Add(y: 1);
            gameObject.transform.DOMove(placedPosition, 0.5f).SetEase(Ease.OutExpo);
        }

        public static void AnimatedRemoval(this GameObject gameObject, Action OnComplete)
        {
            gameObject.transform.DOMove(gameObject.transform.position.Add(y: -3), 0.5f).SetEase(Ease.InExpo).OnComplete(() => OnComplete.Invoke());
        }
    }
}
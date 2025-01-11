using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using DiscoSystem;
using UnityEngine;

namespace ExtensionMethods
{
    public enum ePlacementAnimationType
    {
        BouncyScaleUp,
        Shaky,
        MoveDown,
    }

    public enum eInPlaceAnimationType
    {
        Shake,
        Bounce,
    }
    
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

        public static void AnimatedPlacement(this GameObject gameObject, ePlacementAnimationType animationType)
        {
            switch (animationType)
            {
                case ePlacementAnimationType.BouncyScaleUp:
                    float startScale = 0.8f;
                    gameObject.transform.localScale = new Vector3(startScale, startScale, startScale);
                    gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
                    break;
                case ePlacementAnimationType.Shaky:
                    gameObject.transform.DOShakeScale(0.5f, 0.05f);
                    break;
                case ePlacementAnimationType.MoveDown:
                    Vector3 storedPosition = gameObject.transform.position;
                    gameObject.transform.position = storedPosition.Add(y: 1);
                    gameObject.transform.DOMove(storedPosition, 0.5f).SetEase(Ease.OutExpo);
                    break;
            }
        }
        
        public static void AnimatedRemoval(this GameObject gameObject, Action OnComplete)
        {
            gameObject.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutSine).OnComplete(() => OnComplete.Invoke());
        }
    }
}
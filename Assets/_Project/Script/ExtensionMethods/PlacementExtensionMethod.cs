using System;
using System.Collections.Generic;
using DG.Tweening;
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
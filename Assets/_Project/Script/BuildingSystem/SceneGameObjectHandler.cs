using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace BuildingSystem
{
    public class SceneGameObjectHandler : Singleton<SceneGameObjectHandler>
    {
        [field: SerializeField] public Transform GetPropHolderTransform { get; private set; }
        [field: SerializeField] public Transform GetSurfaceHolderTransform { get; private set; }
        [field: SerializeField] public Transform GetNPCHolderTransform { get; private set; }
        [field: SerializeField] public Transform GetEmployeeHolderTransform { get; private set; }
        [field: SerializeField] public Transform GetFloorTileHolder { get; private set; }
        [field: SerializeField] public Transform GetWallHolder { get; private set; }
        [field: SerializeField] public Transform NullHolder { get; private set; }

        public Transform GetHolderByLayer(ePlacementLayer layer)
        {
            switch (layer)
            {
                case ePlacementLayer.BaseSurface:
                    return GetSurfaceHolderTransform;
                    break;
                case ePlacementLayer.FloorProp:
                case ePlacementLayer.WallProp:
                    return GetPropHolderTransform;
                    break;
            }

            return NullHolder;
        }
        
        // public void ClearMap()
        // {
        //     foreach (var children in GetPropHolderTransform.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        //     foreach (var children in GetSurfaceHolderTransform.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        //     foreach (var children in GetNPCHolderTransform.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        //     foreach (var children in GetEmployeeHolderTransform.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        //     foreach (var children in GetFloorTileHolder.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        //     foreach (var children in GetWallHolder.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        //     foreach (var children in NullHolder.GetComponentsInChildren<Transform>()) Destroy(children.gameObject);
        // }

        /*
         * GameObject
         *  MeshRendererss
         *  Materials
         *
         */
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace BuildingSystem
{
    public class SceneGameObjectHandler : Singleton<SceneGameObjectHandler>
    {
        [field: SerializeField] public Transform GetPropHolderTransform { get; private set; } // Don't Clean
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
        
        public void ClearMap()
        {
            RemoveChildren(GetSurfaceHolderTransform);
            RemoveChildren(GetNPCHolderTransform);
            RemoveChildren(GetEmployeeHolderTransform);
            RemoveChildren(GetFloorTileHolder);
            RemoveChildren(GetWallHolder);
        }

        private void RemoveChildren(Transform holder)
        {
            for (int i = holder.childCount;  i > 0 ; i--)
            {
                Destroy(holder.gameObject);
            }
        }

        /*
         * GameObject
         *  MeshRendererss
         *  Materials
         *
         */
    }
}
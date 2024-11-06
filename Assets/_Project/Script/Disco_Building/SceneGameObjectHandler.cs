using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Disco_Building
{
    public class SceneGameObjectHandler : Singleton<SceneGameObjectHandler>
    {
        [field: SerializeField] public Transform GetFloorPropHolderTransform { get; private set; }
        [field: SerializeField] public Transform GetWallPropHolderTransform { get; private set; }
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
                case ePlacementLayer.FloorProp:
                    return GetFloorPropHolderTransform;
                case ePlacementLayer.WallProp:
                    return GetWallPropHolderTransform;
            }

            return NullHolder;
        }

        public List<Transform> GetExcludeTransformsByLayer(ePlacementLayer layer)
        {
            var transforms = new List<Transform>();

            switch (layer)
            {
                case ePlacementLayer.BaseSurface:
                    transforms.Add(GetFloorPropHolderTransform);
                    break;
                case ePlacementLayer.FloorProp:
                    break;
                case ePlacementLayer.WallProp:
                    transforms.Add(GetFloorPropHolderTransform);
                    transforms.Add(GetWallPropHolderTransform);
                    break;
            }

            return transforms;
        }

        public List<Transform> GetExcludeTransformsByLayer(eMaterialLayer layer)
        {
            var transforms = new List<Transform>();

            switch (layer)
            {
                case eMaterialLayer.FloorMaterial:
                    transforms.Add(GetFloorPropHolderTransform);
                    transforms.Add(GetSurfaceHolderTransform);
                    break;
                case eMaterialLayer.WallMaterial:
                    transforms.Add(GetWallPropHolderTransform);
                    transforms.Add(GetFloorPropHolderTransform);
                    break;
            }

            return transforms;
        }

        private void RemoveChildren(Transform holder)
        {
            foreach (Transform child in holder)
                Destroy(child.gameObject);
        }

    }
}
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
        [field: SerializeField] public Transform GetFloorTileHolder { get; private set; }
        [field: SerializeField] public Transform GetWallHolder { get; private set; }

        /*
         * GameObject
         *  MeshRendererss
         *  Materials
         *
         */
    }
}
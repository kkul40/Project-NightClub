using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterCustomization
{
    [Serializable]
    public class CustomizationItem
    {
        [Serializable]
        public struct Parentable
        {
            public GameObject prefab;
            public HumanBodyBones targetBone;
        }
        public enum AssetType
        {
            Null,
            SkinnedMesh,
            Gameobject,
        }

        public AssetType assetType;

        [Space]
        [HideIf("assetType", AssetType.Gameobject)]
        public SkinnedMeshRenderer[] meshes;
        [HideIf("assetType", AssetType.SkinnedMesh)]
        public Parentable[] objects;
        
        [FormerlySerializedAs("bodyParts")]
        [Space]
        [HideIf("assetType", AssetType.Gameobject)]
        public BodyPartType[] hiddenBodyParts;
    }
}
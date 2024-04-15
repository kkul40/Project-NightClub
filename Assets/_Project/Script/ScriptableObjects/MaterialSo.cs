using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New WallPaper")]
    public class MaterialSo : ItemSo
    {
        public MaterialType MaterialType;
        public FloorType FloorType;
        public Material Material;
    }

    public enum MaterialType
    {
        WallPaper,
        Floor,
    }
}
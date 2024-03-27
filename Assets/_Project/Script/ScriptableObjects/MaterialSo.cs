using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New WallPaper")]
    public class MaterialSo : ItemSo
    {
        public MaterialType MaterialType;
        public Material Material;
    }

    public enum MaterialType
    {
        WallPaper,
        Floor,
    }
}
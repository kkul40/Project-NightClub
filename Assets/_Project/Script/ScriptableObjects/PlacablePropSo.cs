using BuildingSystemFolder;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class PlacablePropSo : ItemSo
    {
        public abstract IBuilder GetBuilder();
    }

    public enum PlacementType
    {
        FloorProp,
        WallProp,
        WallPaper,
        Tile,
    }
}
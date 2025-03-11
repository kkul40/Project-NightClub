using UnityEngine;

namespace Disco_ScriptableObject
{
    public enum StoreItemTypes
    {
        None,
        Chair,
        Table,
        Dj,
        Bar,
        WallPaper,
        DiscoTile,
        FloorTile,
        Decoration,
        Light,
        MapExtend,
    }
    
    public class StoreItemSO : ScriptableObject
    {
        public int ID;
        public string Name;
        [TextArea(3, 3)] 
        public string Description;
        public Sprite Icon;
        public int Price;
        public StoreItemTypes ItemType;

        private void Awake()
        {
            ID = name.GetHashCode() + Name.GetHashCode();
        }
    }
}
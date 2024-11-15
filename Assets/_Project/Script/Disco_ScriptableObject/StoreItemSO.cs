using UnityEngine;

namespace Disco_ScriptableObject
{
    public class StoreItemSO : ScriptableObject
    {
        public int ID;
        public string Name;
        [TextArea(3, 3)] 
        public string Description;
        public Sprite Icon;
        public int Price;

        private void Awake()
        {
            ID = name.GetHashCode() + Name.GetHashCode();
        }
    }
}
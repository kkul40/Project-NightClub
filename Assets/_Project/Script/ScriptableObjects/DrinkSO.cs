using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Drink Data", menuName = "Item/Drink Data")]

    public class DrinkSO : ScriptableObject
    {
        public int ID;
        public Sprite Icon;
        public string Name;
        public int DrinkAmount;
        public float PrepareTime;
        public int Price;
        public GameObject Prefab;
        
        private void Awake()
        {
            ID = name.GetHashCode() + Name.GetHashCode();
        }
    }
}
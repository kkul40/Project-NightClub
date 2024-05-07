using UnityEngine;

namespace ScriptableObjects
{
    public interface IOccupieable
    {
        public NPC Owner { get; set; }
        bool IsOccupied { get; set; }
        public void GetItOccupied(NPC owner);
    }
}
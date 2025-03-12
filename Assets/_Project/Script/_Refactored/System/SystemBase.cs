using Data;
using UnityEngine;

namespace System
{
    public abstract class SystemBase : MonoBehaviour, ISavable
    {
        // Saving and Loading
        public abstract void LoadData(GameData gameData);
        public abstract void SaveData(ref GameData gameData);
    }
}
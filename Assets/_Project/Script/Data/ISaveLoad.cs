using UnityEngine;

namespace Data
{
    public interface ISaveLoad
    {
        void Register()
        {
            SavingAndLoadingSystem.Instance.RegisterForSaveLoad(this);
        }

        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }
}
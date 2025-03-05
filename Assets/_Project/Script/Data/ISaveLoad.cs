using SaveAndLoad;

namespace Data
{

    public enum SavePriority
    {
        Default = 0, // Default
        VeryLow = 1,
        Low = 2,
        Medium = 3, // Game Settings
        High = 4, // System Related
        VeryHigh = 5, // For Data Related
    }
    public interface ISaveLoad
    {
        public SavePriority Priority { get; }
        void Register()
        {
            SavingAndLoadingSystem.Instance.RegisterForSaveLoad(this);
        }

        // TODO Move Load Data Out Of This Put it In The Scene Initializer
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }
}
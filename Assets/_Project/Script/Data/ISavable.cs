namespace Data
{
    public interface ISavable
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }
}
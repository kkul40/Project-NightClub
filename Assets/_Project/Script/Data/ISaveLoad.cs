namespace Data
{
    public interface ISaveLoad
    {
        void Load(GameData gameData);
        void Save(ref GameData gameData);
    }
}
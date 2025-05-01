using System.Collections.Generic;
using Data.New;

namespace SaveAndLoad
{
    public interface IDataService
    {
        public List<string> GetSaveFiles { get; }
        public void Save(NewGameData data, bool overwrite = true);
        public NewGameData Load(string name);
        public void Delete(string name);
        public void DeleteAll();
        public NewGameData GetData(string name);
    }
}


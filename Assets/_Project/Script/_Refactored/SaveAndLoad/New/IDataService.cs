using System.Collections.Generic;
using Data;
using Data.New;

namespace SaveAndLoad.New
{
    public interface IDataService
    {
        public void Save(NewGameData data, bool overwrite = true);
        public NewGameData Load(string name);
        public void Delete(string name);
        public void DeleteAll();
        public NewGameData GetData(string name);
        public IEnumerable<string> ListSaves();
    }
}


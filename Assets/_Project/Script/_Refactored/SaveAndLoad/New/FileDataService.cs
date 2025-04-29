using System.Collections.Generic;
using System.IO;
using Data;
using Data.New;
using UnityEngine;

namespace SaveAndLoad.New
{
    public class FileDataService : IDataService
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        public FileDataService(ISerializer serializer)
        {
            _dataPath = Application.persistentDataPath;
            _fileExtension = ".gamedata";
            _serializer = serializer;
        }

        private string GetPathToFile(string fileName)
        {
            return Path.Combine(_dataPath, string.Concat(fileName, _fileExtension));
        }
        
        public void Save(NewGameData data, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(data.fileName);
            //
            // if (File.Exists(fileLocation))
            // {
            //     throw new IOException($"The file '{data.fileName}.{_fileExtension}' already exists and cannot be overwritten.");
            // }
            //
            File.WriteAllText(fileLocation, _serializer.Serialize(data));
        }

        public NewGameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);
            
            if (File.Exists(fileLocation))
            {
                return _serializer.Deserialize<NewGameData>(File.ReadAllText(fileLocation));
            }
            
            Debug.LogError("Game Data Not Exist");
            return new NewGameData();
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);
            
            if(File.Exists(fileLocation))
                File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach (var filePath in Directory.GetFiles(_dataPath))
                File.Delete(filePath);
        }

        public NewGameData GetData(string name)
        {
            string fileLocation = GetPathToFile(name);
            
            if (File.Exists(fileLocation))
                return _serializer.Deserialize<NewGameData>(File.ReadAllText(fileLocation));

            Debug.Log("File Is Not Exist");
            
            return null;
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (var path in Directory.EnumerateFiles(_dataPath))
            {
                if (Path.GetExtension(path) == _fileExtension)
                    yield return Path.GetFileNameWithoutExtension(path);
            }
        }
    }
}
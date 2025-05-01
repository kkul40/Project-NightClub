using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.New;
using UnityEngine;

namespace SaveAndLoad
{
    public class FileDataService : IDataService
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        private List<string> _saveFiles;
        private bool _isSaveFilesDirty;

        public FileDataService(ISerializer serializer)
        {
            _dataPath = Application.persistentDataPath;
            _fileExtension = ".gamedata";
            _serializer = serializer;
            _saveFiles = new List<string>();
            _isSaveFilesDirty = true;
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
            _isSaveFilesDirty = true;
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

            _isSaveFilesDirty = true;
        }

        public void DeleteAll()
        {
            foreach (var filePath in Directory.GetFiles(_dataPath))
                File.Delete(filePath);
            
            _isSaveFilesDirty = true;
        }

        public NewGameData GetData(string name)
        {
            string fileLocation = GetPathToFile(name);
            
            if (File.Exists(fileLocation))
                return _serializer.Deserialize<NewGameData>(File.ReadAllText(fileLocation));

            Debug.Log("File Is Not Exist");
            
            return null;
        }
        
        public List<string> GetSaveFiles
        {
            get
            {
                if (_isSaveFilesDirty)
                {
                    _saveFiles = ListSaves().ToList();
                    _isSaveFilesDirty = false;
                }

                return _saveFiles;
            }
        }

        private IEnumerable<string> ListSaves()
        {
            foreach (var path in Directory.EnumerateFiles(_dataPath))
            {
                if (Path.GetExtension(path) == _fileExtension)
                    yield return Path.GetFileNameWithoutExtension(path);
            }
        }
    }
}
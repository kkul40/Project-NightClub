using System;
using System.IO;
using Data;
using UnityEngine;

namespace SaveAndLoad
{
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            Debug.Log("Data File Path : " + dataDirPath);
        }

        public GameData Load()
        {
            var fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedGameData = null;

            if (File.Exists(fullPath))
                try
                {
                    var dataToLoad = "";
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadedGameData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to laod data from file : " + fullPath + "\n" + e);
                }

            return loadedGameData;
        }

        public void Save(GameData gameData)
        {
            var fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                var dataToStore = JsonUtility.ToJson(gameData, true);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (var write = new StreamWriter(stream))
                    {
                        write.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file : " + fullPath + "\n" + e);
            }
        }

        public void DeleteData()
        {
            var fullPath = Path.Combine(dataDirPath, dataFileName);

            if (File.Exists(fullPath))
                try
                {
                    File.Delete(fullPath);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to remove data from path : " + fullPath + "\n" + e);
                    throw;
                }
        }
    }
}
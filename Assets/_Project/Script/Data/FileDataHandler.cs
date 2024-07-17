using System;
using System.IO;
using UnityEngine;

namespace Data
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
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedGameData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
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
            }

            return loadedGameData;
        }

        public void Save(GameData gameData)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(gameData, true);
                
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter write = new StreamWriter(stream))
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
    }
}
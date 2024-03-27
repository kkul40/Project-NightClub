using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CharacterCreation;
using PlayerScripts;
using UnityEngine;

namespace Data
{
    public static class SaveSystem
    {
        public static void SaveCustomizedPlayer(PlayerCustomizationLoader playerCustomizationLoader)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/Player.kdata";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerCustomizationIndexData data = new PlayerCustomizationIndexData(playerCustomizationLoader);
            formatter.Serialize(stream, data);
            stream.Close();
            Debug.Log("SAVED");
        }

        public static PlayerCustomizationIndexData LoadCustomizedPlayer()
        {
            string path = Application.persistentDataPath + "/Player.kdata";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerCustomizationIndexData data = formatter.Deserialize(stream) as PlayerCustomizationIndexData;
                stream.Close();

                Debug.Log("LOADED");
                return data;
            }

            return null;
        }
    }
}
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _Project.Script.NewSystem.Data
{
    public static class SaveSystem
    {
        public static void SaveCustomizedPlayer(PlayerCustomization playerCustomization)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/Player.kdata";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerCustomizationIndexData data = new PlayerCustomizationIndexData(playerCustomization);
            formatter.Serialize(stream, data);
            stream.Close();
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

                return data;
            }

            return null;
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BuildingSystem.SO;
using PlayerScripts;
using UnityEngine;

namespace Data
{
    public static class SaveSystem
    {
        public static void SaveCustomizedPlayer(PlayerCustomizationLoader playerCustomizationLoader)
        {
            var formatter = new BinaryFormatter();

            var path = Application.persistentDataPath + "/Player.kdata";
            var stream = new FileStream(path, FileMode.Create);

            var data = new PlayerCustomizationIndexData(playerCustomizationLoader);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerCustomizationIndexData LoadCustomizedPlayer()
        {
            var path = Application.persistentDataPath + "/Player.kdata";

            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);

                var data = formatter.Deserialize(stream) as PlayerCustomizationIndexData;
                stream.Close();
                return data;
            }

            return null;
        }
    }
}
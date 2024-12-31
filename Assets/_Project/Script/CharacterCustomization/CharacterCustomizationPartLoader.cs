using System.Collections.Generic;
using Data;
using DiscoSystem;
using ExtensionMethods;
using ScriptableObjects;
using UnityEngine;

namespace CharacterCustomization
{
    public class CharacterCustomizationPartLoader : Singleton<CharacterCustomizationPartLoader>, ISaveLoad
    {
        public int playerGenderIndex;
        public int playerHairIndex;
        public int playerBeardIndex;
        public int playerAttachmentIndex;
        public int playerEaringIndex;

        [Header("Customization Variables")] public PlayerCustomizationDataSo playerCDS;

        public SkinnedMeshRenderer playerGenderHolder;
        public Transform playerHairHolder;
        public Transform playerBeardHolder;
        public Transform playerAttachmentHolder;
        public Transform playerEaringHolder;

        private void ChangePlayerPart(Transform partHolder, GameObject partPrefab)
        {
            for (var i = partHolder.childCount - 1; i >= 0; i--)
            {
                var child = partHolder.GetChild(i).gameObject;
                Destroy(child);
            }

            if (partPrefab != null)
            {
                var newPart = Instantiate(partPrefab, partHolder);
            }
        }

        public void ChangeIndex(ref int index, ref Transform holder, ref List<ScriptableObjects.CustomizationItem> prefabs, int change)
        {
            index += change;
            if (index > prefabs.Count - 1)
                index = 0;
            else if (index < 0) index = prefabs.Count - 1;

            ChangePlayerPart(holder, prefabs[index].Prefab);
        }

        public void LoadData(GameData gameData)
        {
            var data = gameData.SavedPlayerCustomizationIndexData;
            if (data == null) return;

            playerGenderIndex = data.playerGenderIndex;
            playerHairIndex = data.playerHairIndex;
            playerBeardIndex = data.playerBeardIndex;
            playerAttachmentIndex = data.playerAttachmentIndex;
            playerEaringIndex = data.playerEaringIndex;

            playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
            ChangePlayerPart(playerHairHolder, playerCDS.playerHairPrefabs[playerHairIndex].Prefab);
            ChangePlayerPart(playerBeardHolder, playerCDS.playerBeardPrefabs[playerBeardIndex].Prefab);
            ChangePlayerPart(playerAttachmentHolder, playerCDS.playerAttachtmentPrefabs[playerAttachmentIndex].Prefab);
            ChangePlayerPart(playerEaringHolder, playerCDS.playerEaringPrefabs[playerEaringIndex].Prefab);

            Debug.Log("Character Loaded");
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.SavedPlayerCustomizationIndexData = this.ConvertPlayerCustomizationIndexData();
        }
    }
}
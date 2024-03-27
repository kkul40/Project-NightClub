using System.Collections.Generic;
using Data;
using ScriptableObjects;
using UnityEngine;

namespace CharacterCreation
{
    public class PlayerCustomization : Singleton<PlayerCustomization>
    {
        //TODO CustomizationData dan cek bu degiskenleri
        public int playerGenderIndex = 0;
        public int playerHairIndex = 0;
        public int playerBeardIndex = 0;
        public int playerAttachmentIndex = 0;
        public int playerEaringIndex = 0;
    
        [Header("Customization Variables")] 
        [SerializeField] private PlayerCustomizationDataSo playerCDS;
        [SerializeField] private SkinnedMeshRenderer playerGenderHolder;
        [SerializeField] private Transform playerHairHolder;
        [SerializeField] private Transform playerBeardHolder;
        [SerializeField] private Transform playerAttachmentHolder;
        [SerializeField] private Transform playerEaringHolder;

        private void Start()
        {
            LoadCustomizedPlayer();
        }

        private void LoadCustomizedPlayer()
        {
            PlayerCustomizationIndexData data = SaveSystem.LoadCustomizedPlayer();
            if (data != null)
            {
                playerGenderIndex = data.playerGenderIndex;
                playerHairIndex = data.playerHairIndex;
                playerBeardIndex = data.playerBeardIndex;
                playerAttachmentIndex = data.playerAttachmentIndex;
                playerEaringIndex = data.playerEaringIndex;
            }

            playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
            ChangePlayerPart(playerHairHolder, playerCDS.playerHairPrefabs[playerHairIndex].Prefab);
            ChangePlayerPart(playerBeardHolder, playerCDS.playerBeardPrefabs[playerBeardIndex].Prefab);
            ChangePlayerPart(playerAttachmentHolder, playerCDS.playerAttachtmentPrefabs[playerAttachmentIndex].Prefab);
            ChangePlayerPart(playerEaringHolder, playerCDS.playerEaringPrefabs[playerEaringIndex].Prefab);
        }

        public void OnMaleButton()
        {
            playerGenderIndex = 0;
            playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
        }

        public void OnFemaleButton()
        {
            playerGenderIndex = 1;
            playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
        }

        private void ChangePlayerPart(Transform partHolder,GameObject partPrefab)
        {
            for (int i = partHolder.childCount - 1; i >= 0; i--)
            {
                var child = partHolder.GetChild(i).gameObject;
                Destroy(child);
            }

            if (partPrefab != null)
            {
                var newPart = Instantiate(partPrefab, partHolder);
            }
        }

        private void ChangeIndex(ref int index, ref Transform holder, ref List<CustomizationItem> prefabs, int change)
        {
            index += change;
            if (index > prefabs.Count - 1)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = prefabs.Count -1;
            }
            
            ChangePlayerPart(holder, prefabs[index].Prefab);
        }
        
        
        public void OnHairPreviousButton()
        {
            ChangeIndex(ref playerHairIndex, ref playerHairHolder, ref playerCDS.playerHairPrefabs, -1);
        }

        public void OnHairNextButton()
        {
            ChangeIndex(ref playerHairIndex, ref playerHairHolder, ref playerCDS.playerHairPrefabs, 1);
        }

        public void OnBeardPreviousButton()
        {
            ChangeIndex(ref playerBeardIndex, ref playerBeardHolder, ref playerCDS.playerBeardPrefabs, -1);

        }

        public void OnBeardNextButton()
        {
            ChangeIndex(ref playerBeardIndex, ref playerBeardHolder, ref playerCDS.playerBeardPrefabs, 1);
        }

        public void OnAttachmentPreviousButton()
        {
            ChangeIndex(ref playerAttachmentIndex, ref playerAttachmentHolder, ref playerCDS.playerAttachtmentPrefabs, -1);
        }

        public void OnAttachmentNextButton()
        {
            ChangeIndex(ref playerAttachmentIndex, ref playerAttachmentHolder, ref playerCDS.playerAttachtmentPrefabs, 1);
        }

        public void OnEaringPreviousButton()
        {
            ChangeIndex(ref playerEaringIndex, ref playerEaringHolder, ref playerCDS.playerEaringPrefabs, -1);
        }

        public void OnEaringNextButton()
        {
            ChangeIndex(ref playerEaringIndex, ref playerEaringHolder, ref playerCDS.playerEaringPrefabs, 1);
        }

        public void FinishUpCustomization()
        {
            SaveSystem.SaveCustomizedPlayer(this);
        }
    }
}

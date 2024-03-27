using System.Collections.Generic;
using Data;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace CharacterCreation
{
    public class PlayerCustomizationUI : MonoBehaviour
    {
        [SerializeField] private PlayerCustomizationLoader _playerCL;
        
        public void OnMaleButton()
        {
            _playerCL.playerGenderIndex = 0;
            _playerCL.playerGenderHolder.sharedMesh = _playerCL.playerCDS.playerGenders[_playerCL.playerGenderIndex];
        }

        public void OnFemaleButton()
        {
            _playerCL.playerGenderIndex = 1;
            _playerCL.playerGenderHolder.sharedMesh = _playerCL.playerCDS.playerGenders[_playerCL.playerGenderIndex];
        }
        
        public void OnHairPreviousButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerHairIndex, ref _playerCL.playerHairHolder, ref _playerCL.playerCDS.playerHairPrefabs, -1);
        }

        public void OnHairNextButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerHairIndex, ref _playerCL.playerHairHolder, ref _playerCL.playerCDS.playerHairPrefabs, 1);
        }

        public void OnBeardPreviousButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerBeardIndex, ref _playerCL.playerBeardHolder, ref _playerCL.playerCDS.playerBeardPrefabs, -1);
        }

        public void OnBeardNextButton()
        {
           _playerCL.ChangeIndex(ref _playerCL.playerBeardIndex, ref _playerCL.playerBeardHolder, ref _playerCL.playerCDS.playerBeardPrefabs, 1);
        }

        public void OnAttachmentPreviousButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerAttachmentIndex, ref _playerCL.playerAttachmentHolder, ref _playerCL.playerCDS.playerAttachtmentPrefabs, -1);
        }

        public void OnAttachmentNextButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerAttachmentIndex, ref _playerCL.playerAttachmentHolder, ref _playerCL.playerCDS.playerAttachtmentPrefabs, 1);
        }

        public void OnEaringPreviousButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerEaringIndex, ref _playerCL.playerEaringHolder, ref _playerCL.playerCDS.playerEaringPrefabs, -1);
        }

        public void OnEaringNextButton()
        {
            _playerCL.ChangeIndex(ref _playerCL.playerEaringIndex, ref _playerCL.playerEaringHolder, ref _playerCL.playerCDS.playerEaringPrefabs, 1);
        }

        public void FinishUpCustomization()
        {
            SaveSystem.SaveCustomizedPlayer(_playerCL);
        }
    }
}

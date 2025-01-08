using PlayerScripts;
using UnityEngine;

namespace CharacterCustomization
{
    public class PlayerCustomizationUI : MonoBehaviour
    {
        private CharacterCustomizationPartLoader _characterCl;

        private void Awake()
        {
            _characterCl = CharacterCustomizationPartLoader.Instance;
            if (_characterCl == null) Debug.LogError("PlayerCustomizationLoader Is Missing");
        }

        public void OnMaleButton()
        {
            _characterCl.playerGenderIndex = 0;
            _characterCl.playerGenderHolder.sharedMesh = _characterCl.playerCDS.playerGenders[_characterCl.playerGenderIndex];
        }

        public void OnFemaleButton()
        {
            _characterCl.playerGenderIndex = 1;
            _characterCl.playerGenderHolder.sharedMesh = _characterCl.playerCDS.playerGenders[_characterCl.playerGenderIndex];
        }

        public void OnHairPreviousButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerHairIndex, ref _characterCl.playerHairHolder,
                ref _characterCl.playerCDS.playerHairPrefabs, -1);
        }

        public void OnHairNextButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerHairIndex, ref _characterCl.playerHairHolder,
                ref _characterCl.playerCDS.playerHairPrefabs, 1);
        }

        public void OnBeardPreviousButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerBeardIndex, ref _characterCl.playerBeardHolder,
                ref _characterCl.playerCDS.playerBeardPrefabs, -1);
        }

        public void OnBeardNextButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerBeardIndex, ref _characterCl.playerBeardHolder,
                ref _characterCl.playerCDS.playerBeardPrefabs, 1);
        }

        public void OnAttachmentPreviousButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerAttachmentIndex, ref _characterCl.playerAttachmentHolder,
                ref _characterCl.playerCDS.playerAttachtmentPrefabs, -1);
        }

        public void OnAttachmentNextButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerAttachmentIndex, ref _characterCl.playerAttachmentHolder,
                ref _characterCl.playerCDS.playerAttachtmentPrefabs, 1);
        }

        public void OnEaringPreviousButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerEaringIndex, ref _characterCl.playerEaringHolder,
                ref _characterCl.playerCDS.playerEaringPrefabs, -1);
        }

        public void OnEaringNextButton()
        {
            _characterCl.ChangeIndex(ref _characterCl.playerEaringIndex, ref _characterCl.playerEaringHolder,
                ref _characterCl.playerCDS.playerEaringPrefabs, 1);
        }

        public void FinishUpCustomization()
        {
            // SaveSystem.SaveCustomizedPlayer(_playerCL);
        }
    }
}
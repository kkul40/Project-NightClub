using System;
using CharacterCreation;

namespace Data
{
    [Serializable]
    public class PlayerCustomizationIndexData
    {
        public int playerGenderIndex = 0;   
        public int playerHairIndex = 0;
        public int playerBeardIndex = 0;
        public int playerAttachmentIndex = 0;
        public int playerEaringIndex = 0;

        public PlayerCustomizationIndexData(PlayerCustomization playerCustomization)
        {
            playerGenderIndex = playerCustomization.playerGenderIndex;
            playerHairIndex = playerCustomization.playerHairIndex;
            playerBeardIndex = playerCustomization.playerBeardIndex;
            playerAttachmentIndex = playerCustomization.playerAttachmentIndex;
            playerEaringIndex = playerCustomization.playerEaringIndex;
        }
    }
}
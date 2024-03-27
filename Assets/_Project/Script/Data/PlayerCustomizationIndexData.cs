using System;
using PlayerScripts;

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

        public PlayerCustomizationIndexData(PlayerCustomizationLoader playerCustomizationUI)
        {
            playerGenderIndex = playerCustomizationUI.playerGenderIndex;
            playerHairIndex = playerCustomizationUI.playerHairIndex;
            playerBeardIndex = playerCustomizationUI.playerBeardIndex;
            playerAttachmentIndex = playerCustomizationUI.playerAttachmentIndex;
            playerEaringIndex = playerCustomizationUI.playerEaringIndex;
        }
    }
}
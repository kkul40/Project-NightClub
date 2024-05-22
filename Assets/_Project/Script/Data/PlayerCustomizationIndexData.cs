using System;
using PlayerScripts;

namespace Data
{
    [Serializable]
    public class PlayerCustomizationIndexData
    {
        public int playerGenderIndex;
        public int playerHairIndex;
        public int playerBeardIndex;
        public int playerAttachmentIndex;
        public int playerEaringIndex;

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
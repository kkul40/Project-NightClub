using CharacterCustomization.UI;
using Data.New;
using Root;
using SaveAndLoad.New;
using UI.GamePages.GameButtons;
using UnityEngine;

namespace UI.StartMenu
{
    public class OnFinishUpCustomizationButtonBase : UIButtonBase
    {
        [SerializeField] private PlayerCustomizationUI customizationUI;
        public override void OnHover()
        {
        }

        public override void OnClick()
        {
            NewGameData data = new NewGameData();
            data.fileName = "New Game";
            data.savePlayerCustomizationData = new Save_PlayerEquipmentsData(customizationUI.GetPlayerEquipments);
            
            SaveLoadSystem.Instance.NewGame(data);
            // SceneManager.LoadScene(1);
            SceneLoader.Instance.LoadScene(1);
        }
    }
}
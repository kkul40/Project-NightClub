using System.Linq;
using _Initializer;
using Framework.Context;
using Framework.Mvcs.View;
using SaveAndLoad;
using UI.StartMenu;
using UnityEngine;

namespace UI.GamePages
{
    public class UISavePage : BaseView
    {
        public override PageType PageType { get; protected set; } = PageType.FullPage;
    }
    
    public class UILoadPage : BaseView
    {
        [SerializeField] private Transform contentHolder;
        [SerializeField] private GameObject SaveDataPrefab;
        public override PageType PageType { get; protected set; } = PageType.FullPage;

        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            
            RectTransform rect = transform as RectTransform;
            rect.anchoredPosition = Vector2.zero;
            
           RefreshList();
        }

        private void RefreshList()
        {
            for (int i = contentHolder.childCount - 1; i >= 0; i--)
                Destroy(contentHolder.GetChild(i).gameObject);
            
            var saveNames = SaveLoadSystem.Instance.GetList().ToList();
            
            if (saveNames.Count == 0) return;

            foreach (var sName in saveNames)
            {
                var prefab = Instantiate(SaveDataPrefab, contentHolder);
                UISaveData saveData = prefab.GetComponent<UISaveData>();
                saveData.Init(SaveLoadSystem.Instance.GetGameData(sName));
                saveData.OnDelete += DeleteSave;
                saveData.OnClick += SaveGame;
            }
        }

        private void DeleteSave(string fileName)
        {
            ServiceLocator.Get<UIPageManager>().ShowAreYouSurePopup("Are you sure you want to delete this save file?", () =>
            {
                SaveLoadSystem.Instance.DeleteGame(fileName);
                RefreshList();
            });
        }

        private void SaveGame(string fileName)
        {
            ServiceLocator.Get<UIPageManager>().ShowAreYouSurePopup("Are you sure you want to overwrite this save file?", () =>
            {
                SaveLoadSystem.Instance.SaveGame();
            });
        }

        public void SaveGameAs()
        {
            ServiceLocator.Get<UIPageManager>().ShowTextFieldPopup("Name It", newFileName =>
            {
                foreach (var fileName in SaveLoadSystem.Instance.GetList())
                {
                    if (fileName == newFileName)
                    {
                        // TODO : Same name already exist
                        Debug.Log("Same Name ALready Exisit");
                        return;
                    }
                    
                }
                
                SaveLoadSystem.Instance.SaveGameAs(newFileName);
                RefreshList();
            });
        }

        public override void ToggleView(bool toggle)
        {
            base.ToggleView(toggle);
            if (toggle)
            {
                RefreshList();
            }
        }
    }
    
    
}
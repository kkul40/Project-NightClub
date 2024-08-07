using System.Linq;
using BuildingSystem.SO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace EditorNS.OdinEditor
{
    public class StoreItemDataBase : OdinMenuEditorWindow
    {
        private CreateNewStoreItemSO createNewStoreItemSo;
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("Create New", createNewStoreItemSo);


            var AllItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems").ToList();
            tree.Add("All Items", AllItems);
            
            string defaultPath = "Assets/Resources/ScriptableObjects/StoreItems/";
            tree.AddAllAssetsAtPath("Bars", defaultPath + "Bar", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Chairs", defaultPath + "Chair", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Dance Tiles", defaultPath + "Dance", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Decorations", defaultPath + "Decoration", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("DJs", defaultPath + "DJ", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Floor Tiles", defaultPath + "FloorTile", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Lights", defaultPath + "Light", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("WallPapers", defaultPath + "WallPaper", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("--Test--", defaultPath + "Test", typeof(StoreItemSO));

            return tree;
        }
        
        [MenuItem("Tools/Data/Store Item Data Base")]
        private static void OpenWindow()
        {
            GetWindow<StoreItemDataBase>().Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (createNewStoreItemSo != null)
                DestroyImmediate(createNewStoreItemSo.storeItemSo);
        }
        
        protected override void OnBeginDrawEditors()
        {
            OdinMenuTreeSelection selected = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                // if (SirenixEditorGUI.ToolbarButton("Change Name"))
                // {
                //     StoreItemSO asset = selected.SelectedValue as StoreItemSO;
                //     string path = AssetDatabase.GetAssetPath(asset);
                //     // asset.name
                //     // AssetDatabase.CreateAsset(asset, path);
                //     // AssetDatabase.DeleteAsset(path);
                //     // AssetDatabase.SaveAssets();
                //     
                //     OdinEditorWindow.InspectObject(asset);
                // }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                    ScriptableObjectCreator.ShowDialog<StoreItemSO>("Assets/Resources/ScriptableObjects/StoreItems/", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                
                if (SirenixEditorGUI.ToolbarButton("Delete Current"))
                {
                    StoreItemSO asset = selected.SelectedValue as StoreItemSO;
                    string path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        

        public class CreateNewStoreItemSO
        {
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public StoreItemSO storeItemSo;

            public CreateNewStoreItemSO()
            {
                storeItemSo = ScriptableObject.CreateInstance<StoreItemSO>();
                storeItemSo.Name = "New Store Item";
            }
            

            [Button("Add New Store Item")]
            public void CreateNewAsset()
            {
                AssetDatabase.CreateAsset(storeItemSo,"Assets/Resources/ScriptableObjects/StoreItems/Test" + storeItemSo.Name + ".asset");
                AssetDatabase.SaveAssets();
                
                // Create New Instance of Scriptable
                storeItemSo = ScriptableObject.CreateInstance<StoreItemSO>();
                storeItemSo.Name = "New Store Item";
            }
        }
    }
}
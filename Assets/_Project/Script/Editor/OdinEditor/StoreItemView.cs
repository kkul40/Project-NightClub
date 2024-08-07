using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using ScriptableObjects;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

namespace EditorNS.OdinEditor
{
    public class StoreItemView : OdinMenuEditorWindow
    {
        private CreateNewStoreItemSO createNewStoreItemSo;

        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true;

            #region Load

            // var AllItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems").ToList();
            // tree.AddAllAssetsAtPathCombined("Title", "ScriptableObjects/StoreItems", typeof(StoreItemSO), true);

            string defaultPath = "Assets/Resources/ScriptableObjects/StoreItems/";
            tree.AddAllAssetsAtPath("Drinks", "Assets/Resources/ScriptableObjects/DrinkData", typeof(Drink)); // TODO Baska bir view kullan
            tree.AddAllAssetsAtPath("Bars", defaultPath + "Bar", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Chairs", defaultPath + "Chair", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Dance Tiles", defaultPath + "Dance", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Decorations", defaultPath + "Decoration", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("DJs", defaultPath + "DJ", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Floor Tiles", defaultPath + "FloorTile", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Lights", defaultPath + "Light", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("WallPapers", defaultPath + "WallPaper", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("Extenders", "Assets/Resources/ScriptableObjects/Extender", typeof(StoreItemSO));
            tree.AddAllAssetsAtPath("--Test--", defaultPath + "Test", typeof(StoreItemSO));


            #endregion
            
            tree.EnumerateTree().AddIcons<StoreItemSO>(x => x.Icon);

            return tree;
        }
        
        [MenuItem("Tools/Data/Item Data")]
        private static void OpenWindow()
        {
            var window = GetWindow<StoreItemView>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.Show();
        }
        
        public static string hello = "Hello";


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
                if (SirenixEditorGUI.ToolbarButton("Change Name"))
                {
                    // StoreItemSO asset = selected.SelectedValue as StoreItemSO;
                    // var path = AssetDatabase.GUIDToAssetPath(asset.);
                    // AssetDatabase.RenameAsset(path, asset.Name);
                    // AssetDatabase.SaveAssets();
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create New Drink Item")))
                {
                    ScriptableObjectCreator.ShowDialog<Drink>("Assets/Resources/ScriptableObjects/StoreItems/", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create New StoreItem")))
                {
                    ScriptableObjectCreator.ShowDialog<StoreItemSO>("Assets/Resources/ScriptableObjects/StoreItems/", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
                if (SirenixEditorGUI.ToolbarButton("Delete Current"))
                {
                    var asset = selected.SelectedValue as ScriptableObject;
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
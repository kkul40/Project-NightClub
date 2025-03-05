using System.Character.NPC;
using Disco_ScriptableObject;
using ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace _Project.OdinEditor
{
    public class ConfigDataBase : OdinMenuEditorWindow
    {
        private CreateNewStoreItemSO createNewStoreItemSo;

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("Create New", createNewStoreItemSo);
            tree.AddAllAssetsAtPath("Store Items", "Assets/Resources/DefaultData", typeof(StoreItemSO), true);
            tree.AddAllAssetsAtPath("Animations ", "Assets/Resources/ScriptableObjects/AnimationData", typeof(NpcAnimationSo));
            tree.AddAllAssetsAtPath("Song Data", "Assets/Resources/ScriptableObjects/Song Data", typeof(SongDataSo), true);
            return tree;
        }
        
        [MenuItem("Tools/Data/Config Data")]
        private static void OpenWindow()
        {
            GetWindow<ConfigDataBase>().Show();
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
                AssetDatabase.CreateAsset(storeItemSo, "Assets/Resources/ScriptableObjects/StoreItems/Test" + storeItemSo.Name + ".asset");
                AssetDatabase.SaveAssets();

                // Create New Instance of Scriptable
                storeItemSo = ScriptableObject.CreateInstance<StoreItemSO>();
                storeItemSo.Name = "New Store Item";
            }
        }
    }
}
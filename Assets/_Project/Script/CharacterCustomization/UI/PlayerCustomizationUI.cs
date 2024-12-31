using System.Collections.Generic;
using CharacterCustomization.Scriptables;
using JetBrains.Annotations;
using NPCBehaviour;
using UnityEngine;

namespace CharacterCustomization.UI
{
    public class PlayerCustomizationUI : MonoBehaviour
    {
        public class EquipedItem
        {
            public int index;
            public List<GameObject> instantiatedObjects = new();
            public CustomizationItem assetReference;
        }
        
        public class PlayerEquipments
        {
            public Dictionary<BodyPart, EquipedItem> EquipedItems;
            public PlayerEquipments()
            {
                EquipedItems = new Dictionary<BodyPart, EquipedItem>();
                
                EquipedItems.Add(BodyPart.Head, new EquipedItem());
                EquipedItems.Add(BodyPart.Hair, new EquipedItem());
                EquipedItems.Add(BodyPart.Accessories, new EquipedItem());
                EquipedItems.Add(BodyPart.Top, new EquipedItem());
                EquipedItems.Add(BodyPart.Bottom, new EquipedItem());
                EquipedItems.Add(BodyPart.Shoes, new EquipedItem());
            }
        }

        public enum BodyPart : byte
        {
            Head,
            Hair,
            Accessories,
            Top,
            Bottom,
            Shoes,
        }

        // Variables
        public CustomizationItemsSo _So;

        [SerializeField] private Transform m_Player;
        private Transform m_Armature;
        private Animator m_Animator;
        private SkinnedMeshRenderer m_skinMeshRenderers;
        private BodyPartTag[] m_BodyPartTags;
        private PlayerEquipments m_PlayerEquipments;
        public Dictionary<BodyPart, List<CustomizationItem>> m_ItemGroup;
        
        // Data
        
        // UI
        [SerializeField] private UIItemSwapper Gender;
        [SerializeField] private UIItemSwapper Head;
        [SerializeField] private UIItemSwapper Hair;
        [SerializeField] private UIItemSwapper Accessories;
        [SerializeField] private UIItemSwapper Top;
        [SerializeField] private UIItemSwapper Bottom;
        [SerializeField] private UIItemSwapper Shoes;
        

        private void Awake()
        {
            InitBody(eGenderType.Male);
            
            Gender.OnClickLeft = () => ChangeGender(eGenderType.Male);
            Gender.OnClickRight = () => ChangeGender(eGenderType.Female);
            
            Head.OnClickLeft = () => PreviousBodyPart(BodyPart.Head);
            Head.OnClickRight = () => NextBodyPart(BodyPart.Head);
            
            Hair.OnClickLeft = () => PreviousBodyPart(BodyPart.Hair);
            Hair.OnClickRight = () => NextBodyPart(BodyPart.Hair);
            
            Accessories.OnClickLeft = () => PreviousBodyPart(BodyPart.Accessories);
            Accessories.OnClickRight = () => NextBodyPart(BodyPart.Accessories);
            
            Top.OnClickLeft = () => PreviousBodyPart(BodyPart.Top);
            Top.OnClickRight = () => NextBodyPart(BodyPart.Top);
            
            Bottom.OnClickLeft = () => PreviousBodyPart(BodyPart.Bottom);
            Bottom.OnClickRight = () => NextBodyPart(BodyPart.Bottom);
            
            Shoes.OnClickLeft = () => PreviousBodyPart(BodyPart.Shoes);
            Shoes.OnClickRight = () => NextBodyPart(BodyPart.Shoes);
        }
        
        private void InitBody(eGenderType gender)
        {
            if (m_Player.childCount > 0)
                for (int i = 0; i < m_Player.childCount; i++)
                    Destroy(m_Player.GetChild(i).gameObject);
            
            CustomizationItemsSo.ItemGroup group = new CustomizationItemsSo.ItemGroup();

            switch (gender)
            {
                case eGenderType.Male:
                    group = _So.MaleItems;
                    m_Armature = Instantiate(_So.MaleItems.ArmaturePrefab, m_Player).transform;
                    break;
                case eGenderType.Female:
                    group = _So.FemaleItems;
                    m_Armature = Instantiate(_So.FemaleItems.ArmaturePrefab, m_Player).transform;
                    break;
            }
            
            m_Animator = m_Armature.GetComponent<Animator>();
            
            var meshes = m_Armature.GetComponentsInChildren<SkinnedMeshRenderer>();
            m_skinMeshRenderers = meshes[meshes.Length / 2];
            m_BodyPartTags = m_Armature.GetComponentsInChildren<BodyPartTag>();
            m_PlayerEquipments = new PlayerEquipments();
            
            m_ItemGroup = new Dictionary<BodyPart, List<CustomizationItem>>();
            m_ItemGroup.Add(BodyPart.Head, group.Head);
            m_ItemGroup.Add(BodyPart.Hair, group.Hair);
            m_ItemGroup.Add(BodyPart.Accessories, group.Accessoriees);
            m_ItemGroup.Add(BodyPart.Top, group.Top);
            m_ItemGroup.Add(BodyPart.Bottom, group.Bottom);
            m_ItemGroup.Add(BodyPart.Shoes, group.Shoes);
        }


        private void NextBodyPart(BodyPart bodyPart)
        {
            EquipedItem equipment = m_PlayerEquipments.EquipedItems[bodyPart];
          
            equipment.index = (equipment.index + 1) % m_ItemGroup[bodyPart].Count;
            CustomizationItem customizationItem = m_ItemGroup[bodyPart][equipment.index];
            
            InitItem(customizationItem, equipment);
        }

        private void InitItem(CustomizationItem customization, EquipedItem equipedItem)
        {
            foreach (var o in equipedItem.instantiatedObjects)
                Destroy(o);

            equipedItem.instantiatedObjects = new List<GameObject>();
            
            GameObject go = null;
            SkinnedMeshRenderer skinnedMesh = null;
            foreach(var mesh in customization.meshes)
            {
                //instantiate new gameobject
                go = new GameObject(mesh.name);
                go.transform.SetParent(m_Armature.transform, false);
                equipedItem.instantiatedObjects.Add(go);

                //add the renderer
                skinnedMesh = go.AddComponent<SkinnedMeshRenderer>();
                skinnedMesh.rootBone = m_skinMeshRenderers.rootBone;
                skinnedMesh.bones = m_skinMeshRenderers.bones;
                skinnedMesh.localBounds = m_skinMeshRenderers.localBounds;
                skinnedMesh.sharedMesh = mesh.sharedMesh;
                skinnedMesh.sharedMaterials = mesh.sharedMaterials;
            }

            //instantiate objects, parent to target bones
            foreach(var obj in customization.objects)
            {
                go = Instantiate(obj.prefab, m_Animator.GetBoneTransform(obj.targetBone));
                equipedItem.instantiatedObjects.Add(go);
            }
            
            if(equipedItem.assetReference != null)
                ToggleBodyParts(equipedItem.assetReference, true);
            
            equipedItem.assetReference = customization;
            
            ToggleBodyParts(customization,false);
        }

        private void ToggleBodyParts([CanBeNull] CustomizationItem item, bool toggle)
        {
            if (item == null)
            {
                foreach (var tag in m_BodyPartTags)
                    tag.gameObject.SetActive(toggle);

                return;
            }
            
            foreach (var hiddenPart in item.hiddenBodyParts)
            {
                foreach (var tag in m_BodyPartTags)
                {
                    if (hiddenPart == tag.bodyTag)
                    {
                        tag.gameObject.SetActive(toggle);
                    }
                }
            }
        }

        private void PreviousBodyPart(BodyPart bodyPart)
        {
            
        }

        private void ChangeGender(eGenderType gender)
        {
            InitBody(gender);
            //
            // foreach (var objects in currentEquipped.instantiatedObjects)
            //     Destroy(objects);
            //


            // //initialize all tagged body parts
            // //they be used to disable meshes that are hidden by clothes
            // var bodyparts = body.GetComponentsInChildren<BodyPartTag>();
            // foreach (var part in bodyparts)
            //     m_BodyParts[part.type] = part;
            //
            // var equip = new CustomizationDemo.EquipedItem()
            // {
            //     path = path,
            //     assetReference = null,
            //     instantiatedObjects = new List<GameObject>() { m_Character.gameObject }
            // };
            // InitRenderersForItem(equip);
            // m_Equiped["body"] = equip;
            //
            // //update ui
            // m_UI.SetCategoryValue(m_Categories.IndexOf("body"), path);
            // if (m_UI.IsCustomizationOpen && m_UI.CurrentCategory == "body")
            //     m_UI.SetCustomizationMaterials(equip.renderers);
        }

       
        private int GetLoopIndex(int currentIndex, int maxIndex, int increase)
        {
            currentIndex = maxIndex;

            return currentIndex + increase;
        }
    }
}
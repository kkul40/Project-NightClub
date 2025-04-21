using System.Collections.Generic;
using CharacterCustomization.Scriptables;
using Data;
using JetBrains.Annotations;
using SaveAndLoad;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCustomization.UI
{
    // TODO : Implement Color Change Logic Here Somewhere
    public class PlayerCustomizationUI : MonoBehaviour, ISavable
    {
        public class EquipedItem
        {
            public int index;
            public List<GameObject> instantiatedObjects = new();
            public CustomizationItem assetReference;
        }
        
        public class PlayerEquipments
        {
            public eGenderType PlayerGender;
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

        [Required]
        public CustomizationItemsSo _So;

        [Required]
        [SerializeField] private Transform m_Player;
        private Transform m_Armature;
        private Animator m_Animator;
        private SkinnedMeshRenderer m_skinMeshRenderers;
        private BodyPartTag[] m_BodyPartTags;
        private PlayerEquipments m_PlayerEquipments;
        public Dictionary<BodyPart, List<CustomizationItem>> m_ItemGroup;
        
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
            
            // Equip Defaults
            InitItem(m_ItemGroup[BodyPart.Head][0], m_PlayerEquipments.EquipedItems[BodyPart.Head]);
            InitItem(m_ItemGroup[BodyPart.Hair][0], m_PlayerEquipments.EquipedItems[BodyPart.Hair]);
            // InitItem(m_ItemGroup[BodyPart.Accessories][0], m_PlayerEquipments.EquipedItems[BodyPart.Accessories]);
            InitItem(m_ItemGroup[BodyPart.Top][0], m_PlayerEquipments.EquipedItems[BodyPart.Top]);
            InitItem(m_ItemGroup[BodyPart.Bottom][0], m_PlayerEquipments.EquipedItems[BodyPart.Bottom]);
            InitItem(m_ItemGroup[BodyPart.Shoes][0], m_PlayerEquipments.EquipedItems[BodyPart.Shoes]);
        }

        private void NextBodyPart(BodyPart bodyPart)
        {
            EquipedItem equipment = m_PlayerEquipments.EquipedItems[bodyPart];
          
            equipment.index = (equipment.index + 1) % m_ItemGroup[bodyPart].Count;
            CustomizationItem customizationItem = m_ItemGroup[bodyPart][equipment.index];

            PlayeAnimation(bodyPart);
            InitItem(customizationItem, equipment);
        }
        
        private void PreviousBodyPart(BodyPart bodyPart)
        {
            EquipedItem equipment = m_PlayerEquipments.EquipedItems[bodyPart];
          
            equipment.index = (equipment.index - 1) % m_ItemGroup[bodyPart].Count;
            if (equipment.index < 0)
            {
                equipment.index = m_ItemGroup[bodyPart].Count - 1;
            }
            CustomizationItem customizationItem = m_ItemGroup[bodyPart][equipment.index];
            
            PlayeAnimation(bodyPart);
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
       
        private void ChangeGender(eGenderType gender)
        {
            PlayerGenderAnimation(gender);
            InitBody(gender);
        }

        public void LoadData(GameData gameData)
        {
        }

        private void PlayerGenderAnimation(eGenderType gender)
        {
            // TODO : Add Animations Here
            switch (gender)
            {
                case eGenderType.Male:
                    break;
                case eGenderType.Female:
                    break;
            }
        }
        
        private void PlayeAnimation(BodyPart bodyPart)
        {
            // TODO : Add Animations Here
            switch (bodyPart)
            {
                case BodyPart.Head:
                    break;
                case BodyPart.Hair:
                    break;
                case BodyPart.Accessories:
                    break;
                case BodyPart.Top:
                    break;
                case BodyPart.Bottom:
                    break;
                case BodyPart.Shoes:
                    break;
            }
        }
        
        public void SaveData(ref GameData gameData)
        {
            gameData.SavedPlayerCustomizationIndexData = m_PlayerEquipments.ConvertPlayerCustomizationIndexData();
        }
    }
}
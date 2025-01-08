using Animancer;
using CharacterCustomization;
using CharacterCustomization.Scriptables;
using Data;
using ExtensionMethods;
using JetBrains.Annotations;
using NPCBehaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterCustomizer
    {
        public eGenderType gender;
        private Transform _characterTransform;
        private CustomizationItemsSo _customizationData;
        
        private Transform _armature;
        private Animator _animator;
        private AnimancerComponent _animancer;
        private SkinnedMeshRenderer _skinMeshRenderers;
        private BodyPartTag[] _bodyPartTags;

        
        // Load Player
        public CharacterCustomizer(GameData gameData, CustomizationItemsSo customizationData, Transform characterTransform)
        {
            _characterTransform = characterTransform;
            _customizationData = customizationData;
            
            GameDataExtension.PlayerCustomizationIndexData indexData = gameData.SavedPlayerCustomizationIndexData;
            
            gender = indexData.playerGenderIndex == 0 ? eGenderType.Male : eGenderType.Female;
            CustomizationItemsSo.ItemGroup group = gender == eGenderType.Male ? customizationData.MaleItems : customizationData.FemaleItems;
            InitBody(gender);
            InitItem(group.Head[indexData.playerHeadIndex]);
            InitItem(group.Hair[indexData.playerHairIndex]);
            InitItem(group.Accessoriees[indexData.playerAccessoriesIndex]);
            InitItem(group.Top[indexData.playerTopIndex]);
            InitItem(group.Bottom[indexData.playerBottomIndex]);
            InitItem(group.Shoes[indexData.playerShoesIndex]);
        }

        // Random NPC
        public CharacterCustomizer(eGenderType gender, CustomizationItemsSo customizationData, Transform characterTransform)
        {
            _characterTransform = characterTransform;
            _customizationData = customizationData;
            CustomizationItemsSo.ItemGroup group = gender == eGenderType.Male ? customizationData.MaleItems : customizationData.FemaleItems;
            
            InitBody(gender);
            if(group.Head.Count > 0) 
                InitItem(group.Head[Random.Range(0, group.Head.Count)]);
            if(group.Hair.Count > 0) 
                InitItem(group.Hair[Random.Range(0, group.Hair.Count)]);
            if(group.Accessoriees.Count > 0) 
                InitItem(group.Accessoriees[Random.Range(0, group.Accessoriees.Count)]);
            if(group.Top.Count > 0) 
                InitItem(group.Top[Random.Range(0, group.Top.Count)]);
            if(group.Bottom.Count > 0) 
                InitItem(group.Bottom[Random.Range(0, group.Bottom.Count)]);
            if(group.Shoes.Count > 0) 
                InitItem(group.Shoes[Random.Range(0, group.Shoes.Count)]);
        }
        
        private void InitBody(eGenderType gender)
        {
            if (_characterTransform.childCount > 0)
                for (int i = 0; i < _characterTransform.childCount; i++)
                    Object.Destroy(_characterTransform.GetChild(i).gameObject);
            
            CustomizationItemsSo.ItemGroup group = new CustomizationItemsSo.ItemGroup();

            switch (gender)
            {
                case eGenderType.Male:
                    group = _customizationData.MaleItems;
                    _armature = Object.Instantiate(_customizationData.MaleItems.ArmaturePrefab, _characterTransform).transform;
                    break;
                case eGenderType.Female:
                    group = _customizationData.FemaleItems;
                    _armature = Object.Instantiate(_customizationData.FemaleItems.ArmaturePrefab, _characterTransform).transform;
                    break;
            }
            
            _animator = _armature.GetComponent<Animator>();
            _animancer = _armature.GetComponent<AnimancerComponent>();
            
            var meshes = _armature.GetComponentsInChildren<SkinnedMeshRenderer>();
            _skinMeshRenderers = meshes[meshes.Length / 2];
            _bodyPartTags = _armature.GetComponentsInChildren<BodyPartTag>();
        }
        
        private void InitItem(CustomizationItem customization)
        {
            GameObject go = null;
            SkinnedMeshRenderer skinnedMesh = null;
            foreach(var mesh in customization.meshes)
            {
                //instantiate new gameobject
                go = new GameObject(mesh.name);
                go.transform.SetParent(_armature.transform, false);

                //add the renderer
                skinnedMesh = go.AddComponent<SkinnedMeshRenderer>();
                skinnedMesh.rootBone = _skinMeshRenderers.rootBone;
                skinnedMesh.bones = _skinMeshRenderers.bones;
                skinnedMesh.localBounds = _skinMeshRenderers.localBounds;
                skinnedMesh.sharedMesh = mesh.sharedMesh;
                skinnedMesh.sharedMaterials = mesh.sharedMaterials;
            }

            //instantiate objects, parent to target bones
            foreach(var obj in customization.objects)
            {
                go = Object.Instantiate(obj.prefab, _animator.GetBoneTransform(obj.targetBone));
            }
            
            ToggleBodyParts(customization,false);
        }
        
        private void ToggleBodyParts([CanBeNull] CustomizationItem item, bool toggle)
        {
            if (item == null)
            {
                foreach (var tag in _bodyPartTags)
                    tag.gameObject.SetActive(toggle);

                return;
            }
            
            foreach (var hiddenPart in item.hiddenBodyParts)
            {
                foreach (var tag in _bodyPartTags)
                {
                    if (hiddenPart == tag.bodyTag)
                    {
                        tag.gameObject.SetActive(toggle);
                    }
                }
            }
        }

        public Animator GetAnimator => _animator;
        public AnimancerComponent GetAnimancer => _animancer;
        public Transform GetArmature => _armature;
    }
}
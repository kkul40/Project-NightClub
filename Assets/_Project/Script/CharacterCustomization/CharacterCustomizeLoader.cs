using Animancer;
using CharacterCustomization.Scriptables;
using Data;
using ExtensionMethods;
using JetBrains.Annotations;
using NPCBehaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCustomization
{
    public class CharacterCustomizeLoader : MonoBehaviour
    {
        [Required] 
        [SerializeField] private CustomizationItemsSo _So;

        public eGenderType gender;
        private Transform _armature;
        private Animator _animator;
        private AnimancerComponent _animancer;
        private SkinnedMeshRenderer _skinMeshRenderers;
        private BodyPartTag[] _bodyPartTags;

        public void Init(GameData gameData)
        {
            GameDataExtension.PlayerCustomizationIndexData indexData = gameData.SavedPlayerCustomizationIndexData;
            
            gender = indexData.playerGenderIndex == 0 ? eGenderType.Male : eGenderType.Female;
            CustomizationItemsSo.ItemGroup group = gender == eGenderType.Male ? _So.MaleItems : _So.FemaleItems;
            InitBody(gender);
            InitItem(group.Head[indexData.playerHeadIndex]);
            InitItem(group.Hair[indexData.playerHairIndex]);
            InitItem(group.Accessoriees[indexData.playerAccessoriesIndex]);
            InitItem(group.Top[indexData.playerTopIndex]);
            InitItem(group.Bottom[indexData.playerBottomIndex]);
            InitItem(group.Shoes[indexData.playerShoesIndex]);
        }
        
        private void InitBody(eGenderType gender)
        {
            if (transform.childCount > 0)
                for (int i = 0; i < transform.childCount; i++)
                    Destroy(transform.GetChild(i).gameObject);
            
            CustomizationItemsSo.ItemGroup group = new CustomizationItemsSo.ItemGroup();

            switch (gender)
            {
                case eGenderType.Male:
                    group = _So.MaleItems;
                    _armature = Instantiate(_So.MaleItems.ArmaturePrefab, transform).transform;
                    break;
                case eGenderType.Female:
                    group = _So.FemaleItems;
                    _armature = Instantiate(_So.FemaleItems.ArmaturePrefab, transform).transform;
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
                go = Instantiate(obj.prefab, _animator.GetBoneTransform(obj.targetBone));
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
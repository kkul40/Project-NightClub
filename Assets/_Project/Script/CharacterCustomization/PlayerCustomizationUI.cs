using System.Collections.Generic;
using NPCBehaviour;
using Rukha93.ModularAnimeCharacter.Customization;
using UnityEngine;

namespace CharacterCustomization
{
    public class PlayerCustomizationUI : MonoBehaviour
    {
        private enum BodyPartTypes : byte
        {
            Gender,
            Hair,
            Accessories,
            Top,
            Bottom,
            Shoes,
        }

        public GameObject MaleCharacter;
        public GameObject FemaleCharacter;

        [SerializeField] private Transform m_Player;
        private Animator m_Animator;
        private SkinnedMeshRenderer m_skinMeshRenderers;
        
        
        [SerializeField] private UIItemSwapper Gender;
        [SerializeField] private UIItemSwapper Hair;
        [SerializeField] private UIItemSwapper Accessories;
        [SerializeField] private UIItemSwapper Top;
        [SerializeField] private UIItemSwapper Bottom;
        [SerializeField] private UIItemSwapper Shoes;

        public CustomizationItemAsset[] _skinnedMeshRenderers;

        private EquipedItem currentEquipped;
        public class EquipedItem
        {
            public int index;
            public List<GameObject> instantiatedObjects = new List<GameObject>();
            public CustomizationItemAsset assetReference = new CustomizationItemAsset();

            //for material customization
            public Renderer[] renderers = new Renderer[10];
        }

        private void Awake()
        {
            currentEquipped = new EquipedItem();
            
            ChangeGender(eGenderType.Female);
            
            Gender.OnClickLeft = () => ChangeGender(eGenderType.Male);
            Gender.OnClickRight = () => ChangeGender(eGenderType.Female);
            
            Hair.OnClickLeft = () => PreviousBodyPart(BodyPartTypes.Hair);
            Hair.OnClickRight = () => NextBodyPart(BodyPartTypes.Hair);
            
            Accessories.OnClickLeft = () => PreviousBodyPart(BodyPartTypes.Accessories);
            Accessories.OnClickRight = () => NextBodyPart(BodyPartTypes.Accessories);
            
            Top.OnClickLeft = () => PreviousBodyPart(BodyPartTypes.Top);
            Top.OnClickRight = () => NextBodyPart(BodyPartTypes.Top);
            
            Bottom.OnClickLeft = () => PreviousBodyPart(BodyPartTypes.Bottom);
            Bottom.OnClickRight = () => NextBodyPart(BodyPartTypes.Bottom);
            
            Shoes.OnClickLeft = () => PreviousBodyPart(BodyPartTypes.Shoes);
            Shoes.OnClickRight = () => NextBodyPart(BodyPartTypes.Shoes);
        }

        private void NextBodyPart(BodyPartTypes bodyPartTypes)
        {
            Debug.Log("Clicckeeed");

            foreach (var objects in currentEquipped.instantiatedObjects)
                Destroy(objects);
            
            currentEquipped.index = (currentEquipped.index + 1) % (_skinnedMeshRenderers.Length - 1);
            // Instantiate(_skinnedMeshRenderers[index], m_Player);
            
            GameObject go = null;
            SkinnedMeshRenderer skinnedMesh = null;
            foreach(var mesh in _skinnedMeshRenderers[currentEquipped.index].meshes)
            {
                //instantiate new gameobject
                go = new GameObject(mesh.name);
                go.transform.SetParent(m_Player.transform, false);
                currentEquipped.instantiatedObjects.Add(go);

                //add the renderer
                skinnedMesh = go.AddComponent<SkinnedMeshRenderer>();
                skinnedMesh.rootBone = m_skinMeshRenderers.rootBone;
                skinnedMesh.bones = m_skinMeshRenderers.bones;
                skinnedMesh.localBounds = m_skinMeshRenderers.localBounds;
                skinnedMesh.sharedMesh = mesh.sharedMesh;
                skinnedMesh.sharedMaterials = mesh.sharedMaterials;
            }

            //instantiate objects, parent to target bones
            foreach(var obj in _skinnedMeshRenderers[currentEquipped.index].objects)
            {
                go = Instantiate(obj.prefab, m_Animator.GetBoneTransform(obj.targetBone));
                currentEquipped.instantiatedObjects.Add(go);
            }
        }

        private void PreviousBodyPart(BodyPartTypes bodyPartTypes)
        {
            
        }

        private void ChangeGender(eGenderType gender)
        {
            GameObject body = null;

            if(m_Player.childCount > 0)
                Destroy(m_Player.GetChild(0).gameObject);
            
            switch (gender)
            {
                case eGenderType.Male:
                    body = Instantiate(MaleCharacter, m_Player);
                    break;
                case eGenderType.Female:
                    body = Instantiate(FemaleCharacter, m_Player);
                    break;
            }
            
            //instantiate the body prefab and store the animator
            m_Animator = body.GetComponent<Animator>();
            
            //get a random body mesh to be used as reference
            var meshes = m_Player.GetComponentsInChildren<SkinnedMeshRenderer>();
            m_skinMeshRenderers = meshes[meshes.Length / 2];
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
    }
}
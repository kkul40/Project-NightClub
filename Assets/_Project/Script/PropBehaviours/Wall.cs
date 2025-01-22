using System;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using UnityEngine;

namespace PropBehaviours
{
    public class Wall : MonoBehaviour, IInteractable, IChangableMaterial
    {
        public virtual GameObject mGameobject { get; private set; }
        public virtual bool IsInteractable { get; } = true;
        public virtual bool IsAnimatable { get; } = true;
        public virtual eInteraction Interaction { get; } = eInteraction.None;

        private void Awake()
        {
            mGameobject = gameObject;
        }

        public virtual void OnFocus()
        {
        }

        public virtual void OnOutFocus()
        {
        }

        public virtual void OnClick()
        {
        }

        public int assignedMaterialID { get; protected set; }
        public virtual MeshRenderer _meshRenderer => transform.GetComponentInChildren<MeshRenderer>();

        public eMaterialLayer MaterialLayer { get; } = eMaterialLayer.WallMaterial;

        public virtual Material CurrentMaterial => _meshRenderer.materials[1];

        public virtual void UpdateMaterial(MaterialItemSo materialItemSo)
        {
            var materials = _meshRenderer.materials;
            materials[1] = materialItemSo.Material;
            _meshRenderer.materials = materials;
            assignedMaterialID = materialItemSo.ID;
        }
    }
}
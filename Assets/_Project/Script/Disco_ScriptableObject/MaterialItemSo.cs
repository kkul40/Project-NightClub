using Data;
using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(fileName = "New Material Item", menuName = "Item/Material Item")]
    public class MaterialItemSo : StoreItemSO
    {
        public Material Material;
        public eMaterialLayer MaterialLayer;
    }
}
using Data;
using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(menuName = "New Item/New Material Item")]
    public class MaterialItemSo : StoreItemSO
    {
        public Material Material;
        public eMaterialLayer MaterialLayer;
    }
}
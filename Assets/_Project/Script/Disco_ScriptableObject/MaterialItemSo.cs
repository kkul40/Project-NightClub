using UnityEngine;

namespace BuildingSystem.SO
{
    [CreateAssetMenu(menuName = "New Item/New Material Item")]
    public class MaterialItemSo : StoreItemSO
    {
        public Material Material;
        public eMaterialLayer MaterialLayer;
    }
}
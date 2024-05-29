using UnityEngine;

namespace _1BuildingSystemNew.SO
{
    [CreateAssetMenu(menuName = "New Item/New Material Item")]
    public class MaterialItemSo : StoreItemSO
    {
        public Material Material;
        public eMaterialLayer MaterialLayer;
    }
}
using _1BuildingSystemNew.Builders;
using UnityEngine;

namespace _1BuildingSystemNew
{
    public static class BuildingMethodFactory
    {
        public static IBuildingMethod GetBuildingMethod(StoreItemSO storeItemSo)
        {
            if (storeItemSo is PlacementItemSO placementItemSo)
            {
                return new PlacementBuilder();
            }

            return null;
        }
    }
}
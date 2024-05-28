using _1BuildingSystemNew;
using _1BuildingSystemNew.Builders;
using UnityEngine;
using UnityEngine.Serialization;

namespace Testing
{
    public class UISlot : MonoBehaviour
    {
        public StoreItemSO storeItemSo;
        
        public void OnClick()
        {
            BuildingManager.Instance.StartBuild(storeItemSo);
        }
    }
}
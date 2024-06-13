using BuildingSystem;
using BuildingSystem.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Testing
{
    public class UISlot : MonoBehaviour
    {
        public StoreItemSO storeItemSo;
        
        public void OnClick()
        {
            // BuildingManager.Instance.StartBuild(storeItemSo);
        }
    }
}
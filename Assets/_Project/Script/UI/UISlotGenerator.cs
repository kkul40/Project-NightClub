using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using UnityEngine;

namespace UI
{
    public class UISlotGenerator : MonoBehaviour
    {
        // TODO daha sonra ui slotu scriptableObject ten cek
        [SerializeField] private GameObject UISlotPrefab;
        [SerializeField] private Transform SlotHolder;
        [SerializeField] private string ItemSoPath;

        [SerializeField] private List<StoreItemSO> _placableSoList;

        private void Start()
        {
            _placableSoList = Resources.LoadAll<StoreItemSO>(ItemSoPath).ToList();

            foreach (var placablePropSo in _placableSoList)
            {
                var temp = Instantiate(UISlotPrefab, SlotHolder);
                temp.GetComponent<UISlot>().Init(placablePropSo);
            }
        }
    }
}
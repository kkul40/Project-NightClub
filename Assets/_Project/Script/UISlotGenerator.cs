using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class UISlotGenerator : MonoBehaviour
{
    // TODO daha sonra ui slotu scriptableObject ten cek
    [SerializeField] private GameObject UISlotPrefab;
    [SerializeField] private Transform SlotHolder;
    [SerializeField] private string ItemSoPath;

    [SerializeField] private List<ItemSo> _itemSoList;

    private void Start()
    {
        _itemSoList = Resources.LoadAll<ItemSo>(ItemSoPath).ToList();

        foreach (var itemSo in _itemSoList)
        {
            var temp = Instantiate(UISlotPrefab, SlotHolder);
            temp.GetComponent<UISlot>().SetUp(itemSo);
        }
    }
}
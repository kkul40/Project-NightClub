using Disco_ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace System.Building_System.View
{
    public class UI_CargoItemSlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        public int Amount;
        public StoreItemSO StoreItemSo;

        public void Initialize(StoreItemSO storeItemSo, int amount)
        {
            Amount = amount;
            StoreItemSo = storeItemSo;
            _iconImage.sprite = storeItemSo.Icon;
        }
    }
}
using Disco_ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace DiscoSystem.Building_System.View
{
    public class UI_StoreItemSlot : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private Image _iconImage;
        public StoreItemSO StoreItemSo;

        public void Initialize(StoreItemSO storeItemSo)
        {
            StoreItemSo = storeItemSo;
            _button = GetComponent<Button>();
            _iconImage.sprite = storeItemSo.Icon;
        }
    }
}
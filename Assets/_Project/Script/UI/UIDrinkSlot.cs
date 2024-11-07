using System;
using Disco_ScriptableObject;
using PropBehaviours;
using ScriptableObjects;
using TMPro;
using UI.GamePages;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIDrinkSlot : MonoBehaviour
    {
        private DrinkSO _drinkSo;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;

        private UIBarPage _barPage;

        public void Init(DrinkSO drinkSoToStore, UIBarPage barPage)
        {
            _drinkSo = drinkSoToStore;
            _image.sprite = drinkSoToStore.Icon;
            _nameText.text = drinkSoToStore.Name;
            _priceText.text = drinkSoToStore.Price.ToString();
            
            _barPage = barPage;
        }

        public void OnClick()
        {
            _barPage.SelectADrink(_drinkSo);
        }
    }
}
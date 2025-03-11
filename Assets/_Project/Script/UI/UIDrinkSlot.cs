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
        [SerializeField] private Image _imageShadow;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;

        private UIPickADrinkPage _pickADrinkPage;

        public void Init(DrinkSO drinkSoToStore, UIPickADrinkPage pickADrinkPage)
        {
            _drinkSo = drinkSoToStore;
            _image.sprite = drinkSoToStore.Icon;
            _nameText.text = drinkSoToStore.Name;
            _priceText.text = drinkSoToStore.Price.ToString();

            _imageShadow.sprite = drinkSoToStore.Icon;
            _imageShadow.color = Color.black;
            
            _pickADrinkPage = pickADrinkPage;
        }

        public void OnClick()
        {
            _pickADrinkPage.SelectADrink(_drinkSo);
        }
    }
}
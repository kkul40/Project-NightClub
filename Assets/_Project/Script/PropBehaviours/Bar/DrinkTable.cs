using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace PropBehaviours
{
    public class DrinkTable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform EmptyTransfrom;
        [SerializeField] private Transform DrinkHolder;

        private IBar _bar;
        private DrinkSO _drinkSo;
        public int drinksLeft;
        public bool IsOutOfDrinks = true;

        public void SetUpTable(DrinkSO drinkSo, IBar bar)
        {
            _drinkSo = drinkSo;
            _bar = bar;
            drinksLeft = drinkSo.DrinkAmount;

            for (var i = DrinkHolder.childCount - 1; i > 0; i--) Destroy(DrinkHolder.GetChild(i).gameObject);

            var d = Instantiate(drinkSo.Prefab, DrinkHolder);

            IsOutOfDrinks = false;
            EmptyTransfrom.gameObject.SetActive(false);
            DrinkHolder.gameObject.SetActive(true);
        }

        public void RemoveDrink(int removeAmount)
        {
            if (drinksLeft - removeAmount < 0)
            {
                Debug.LogError("Not Enough Drink Check This Code Later");
            }
            
            drinksLeft -= removeAmount;
            if (drinksLeft <= 0) EmptyTable();
        }

        public void CleanUP()
        {
            transform.gameObject.SetActive(false);
        }

        private void EmptyTable()
        {
            DrinkHolder.gameObject.SetActive(false);
            EmptyTransfrom.gameObject.SetActive(true);
            drinksLeft = 0;
            IsOutOfDrinks = true;
        }
        
        #region IInteractable
        public bool IsInteractable { get; } = true;
        public eInteraction Interaction { get; } = eInteraction.Interactable;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            // Clean UP
            // TODO Show Drink Info
        }

        #endregion
    }
}
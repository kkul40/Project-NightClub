using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class DrinkTable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform EmptyTransfrom;
        [SerializeField] private Transform DrinkHolder;

        private Drink drink;
        public int drinkAmount;
        private bool isClicked;

        public eInteraction Interaction { get; } = eInteraction.Interactable;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            if (EmptyTransfrom.gameObject.activeInHierarchy) transform.gameObject.SetActive(false);
        }

        public void SetUpTable(Drink drink)
        {
            this.drink = drink;
            drinkAmount = drink.DrinkAmount;

            for (var i = DrinkHolder.childCount - 1; i > 0; i--) Destroy(DrinkHolder.GetChild(i).gameObject);

            var d = Instantiate(drink.Prefab, DrinkHolder);

            EmptyTransfrom.gameObject.SetActive(false);
            DrinkHolder.gameObject.SetActive(true);
        }

        public void GetDrink()
        {
            drinkAmount--;

            if (drinkAmount <= 0) EmptyTable();
        }

        public void EmptyTable()
        {
            DrinkHolder.gameObject.SetActive(false);
            EmptyTransfrom.gameObject.SetActive(true);
            isClicked = true;
        }
    }
}
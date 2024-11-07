using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class DrinkTable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform EmptyTransfrom;
        [SerializeField] private Transform DrinkHolder;

        private IBar _bar;
        private DrinkSO _drinkSo;
        public int drinkAmount;
        private bool isClicked;

        public bool isFinished = true;

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

        public void SetUpTable(DrinkSO drinkSo, IBar bar)
        {
            this._drinkSo = drinkSo;
            _bar = bar;
            drinkAmount = drinkSo.DrinkAmount;

            for (var i = DrinkHolder.childCount - 1; i > 0; i--) Destroy(DrinkHolder.GetChild(i).gameObject);

            var d = Instantiate(drinkSo.Prefab, DrinkHolder);

            isFinished = false;
            EmptyTransfrom.gameObject.SetActive(false);
            DrinkHolder.gameObject.SetActive(true);
        }

        public void GetDrink()
        {
            drinkAmount--;
            if (drinkAmount <= 0) EmptyTable();
        }

        public void CleanUP()
        {
            if (EmptyTransfrom.gameObject.activeInHierarchy) transform.gameObject.SetActive(false);
        }

        public void EmptyTable()
        {
            DrinkHolder.gameObject.SetActive(false);
            EmptyTransfrom.gameObject.SetActive(true);
            isClicked = true;
            isFinished = true;
        }
    }
}
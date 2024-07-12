using BuildingSystem;
using Data;
using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class Bar : IPropUnit, IBar
    {
        [SerializeField] private GameObject DrinkTablePrefab;
        [SerializeField] private Transform TezgahTransform;
        [SerializeField] private Transform CustomerWaitTransform;

        private DrinkTable _drinkTable;

        public Transform GetTezgahTransform => TezgahTransform;

        public bool HasTable => _drinkTable.gameObject.activeInHierarchy;

        public int AvaliableDrinkCount => _drinkTable.drinkAmount;

        public bool HasDrinks => _drinkTable.drinkAmount > 0 ? true : false;

        public Transform WaitPosition => CustomerWaitTransform;

        public void GetDrink()
        {
            _drinkTable.GetDrink();
        }

        public override void Initialize(int ID, Vector3Int cellPosition, RotationData rotationData,
            ePlacementLayer placementLayer)
        {
            base.Initialize(ID, cellPosition, rotationData, placementLayer);
            var d = Instantiate(DrinkTablePrefab, GetTezgahTransform);
            d.transform.position = GetTezgahTransform.position;
            _drinkTable = d.GetComponent<DrinkTable>();
            _drinkTable.gameObject.SetActive(false);
        }

        public void SetDrinkTable(Drink drink)
        {
            Debug.Log("DrinkTable Sett");
            _drinkTable.gameObject.SetActive(true);
            _drinkTable.SetUpTable(drink);
        }
    }

    public interface IBar
    {
        public int AvaliableDrinkCount { get; }
        public bool HasDrinks { get; }
        public Transform WaitPosition { get; }
        public void GetDrink();
    }
}
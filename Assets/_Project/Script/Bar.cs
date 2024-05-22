using ScriptableObjects;
using UnityEngine;

public class Bar : Prop, IBar
{
    [SerializeField] private GameObject DrinkTablePrefab;
    [SerializeField] private Transform TezgahTransform;
    [SerializeField] private Transform CustomerWaitTransform;

    private DrinkTable _drinkTable;

    public Transform GetTezgahTransform => TezgahTransform;

    public bool HasTable => _drinkTable.gameObject.activeInHierarchy;

    public int AvaliableDrinkCount { get; private set; }

    public bool HasDrinks => AvaliableDrinkCount > 0 ? true : false;

    public Transform WaitPosition => CustomerWaitTransform;

    public void GetDrink()
    {
        _drinkTable.GetDrink();
    }

    public void DecreaseDrinkCount()
    {
        AvaliableDrinkCount--;
    }

    public override void Initialize(IPlaceableItemData placableItemDataSo, Vector3Int cellPosition, Direction direction)
    {
        base.Initialize(placableItemDataSo, cellPosition, direction);

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
        AvaliableDrinkCount = drink.DrinkAmount;
    }
}

public interface IBar
{
    public int AvaliableDrinkCount { get; }
    public bool HasDrinks { get; }
    public Transform WaitPosition { get; }
    public void GetDrink();
    public void DecreaseDrinkCount();
}
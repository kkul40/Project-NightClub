using System;
using ScriptableObjects;
using UnityEngine;

public class DrinkTable : MonoBehaviour, ICursorInteraction
{
    [SerializeField] private Transform EmptyTransfrom;
    [SerializeField] private Transform DrinkHolder;
    
    private Drink drink;
    private int drinkAmount;
    private bool isClicked = false;

    public void SetUpTable(Drink drink)
    {
        Debug.Log("Table set");
        this.drink = drink;
        drinkAmount = drink.DrinkAmount;

        for (int i = DrinkHolder.childCount - 1; i > 0; i--)
        {
            Destroy(DrinkHolder.GetChild(i).gameObject);
        }
        
        var d = Instantiate(drink.Prefab, DrinkHolder);
        
        EmptyTransfrom.gameObject.SetActive(false);
        DrinkHolder.gameObject.SetActive(true);
    }
    
    public void GetDrink()
    {
        drinkAmount--;
        
        if (drinkAmount <= 0)
        {
            EmptyTable();
        }
    }

    public void EmptyTable()
    {
        DrinkHolder.gameObject.SetActive(false);
        EmptyTransfrom.gameObject.SetActive(true);
        isClicked = true;
    }

    public void OnFocus()
    {
    }

    public void OnOutFocus()
    {
        
    }

    public void OnClick()
    {
        if (EmptyTransfrom.gameObject.activeInHierarchy)
        {
            transform.gameObject.SetActive(false);
        }
    }
}

public interface ICursorInteraction
{
    public void OnFocus();
    public void OnOutFocus();
    public void OnClick();
}
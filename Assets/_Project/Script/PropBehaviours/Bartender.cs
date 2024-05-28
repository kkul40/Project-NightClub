using System.Collections;
using ScriptableObjects;
using UnityEngine;

public class Bartender : MonoBehaviour, IInteractable
{
    public Drink drink; // Test
    public Bar bar;

    private Coroutine _coroutine;
    private bool isClicked => _coroutine != null;

    public eInteraction Interaction { get; } = eInteraction.Interactable;

    public void OnFocus()
    {
    }

    public void OnOutFocus()
    {
    }

    public void OnClick()
    {
        if (!isClicked && !bar.HasTable)
        {
            Debug.Log("Bartender Clicked");
            _coroutine = StartCoroutine(PrepareDrinkCo());
        }
    }

    private IEnumerator PrepareDrinkCo()
    {
        yield return new WaitForSeconds(drink.PrepareTime);
        bar.SetDrinkTable(drink);

        _coroutine = null;
    }
}
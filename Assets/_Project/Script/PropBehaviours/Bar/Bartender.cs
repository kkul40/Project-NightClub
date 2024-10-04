using BuildingSystem;
using Data;
using NPC_Stuff;
using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class Bartender : MonoBehaviour
    {
        //     private BarMediator BarMediator;
        //     
        //     
        //     public Drink drink; // Test
        //     public Bar bar;
        //
        //     private Coroutine _coroutine;
        //     private bool isBusy => _coroutine != null;
        //
        //     public IPathFinder PathFinder;
        //
        //     public eInteraction Interaction { get; } = eInteraction.Interactable;
        //
        //     public void Init()
        //     {
        //         PathFinder = new BartenderPathFinder();
        //     }
        //
        //     public void OnFocus()
        //     {
        //     }
        //
        //     public void OnOutFocus()
        //     {
        //     }
        //
        //     public void OnClick()
        //     {
        //         if (!isBusy && !bar.HasTable)
        //         {
        //             Debug.Log("Bartender Clicked");
        //             _coroutine = StartCoroutine(PrepareDrinkCo());
        //             BarMediator.Notify(this);
        //         }
        //     }
        //
        //     private IEnumerator PrepareDrinkCo()
        //     {
        //         yield return new WaitForSeconds(drink.PrepareTime);
        //         bar.SetDrinkTable(drink);
        //
        //         _coroutine = null;
        //     }
    }
}
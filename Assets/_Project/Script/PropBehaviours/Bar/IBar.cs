using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public interface IBar : IID
    {
        Transform BartenderWaitPosition { get; }
        Transform CustomerWaitPosition { get; }
        Transform CounterPlacePosition { get; }
        bool HasDrinks { get; }
        bool IsBusy { get; set; }
        bool IsServing { get; set; }
        void GetDrink();
        void CreateDrinks(DrinkSO drinkToCreate);
        void CleanBar();
    }
}
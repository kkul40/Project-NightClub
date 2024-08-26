using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public interface IBar : IID
    {
        Transform BartenderWaitPosition { get; }
        Transform CustomerWaitPosition { get; }
        Transform CounterPlacePosition { get; }
        DrinkTable DrinkTable { get; }
        int DrinkAmount { get; }
        bool HasDrinks { get; set; }
        void GetDrink();
        void CreateDrinks();
    }
}
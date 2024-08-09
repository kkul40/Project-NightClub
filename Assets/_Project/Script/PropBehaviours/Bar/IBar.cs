using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public interface IBar : IID
    {
        Transform BartenderWaitPosition { get; }
        Transform CustomerWaitPosition { get; }
        Transform CounterPlacePosition { get; }
        Drink DrinkData { get; }
        bool HasDrinks { get; set; }
        void CreateDrinks();
    }
}
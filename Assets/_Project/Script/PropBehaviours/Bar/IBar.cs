using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public interface IBar
    {
        Transform BartenderWaitPosition { get; }
        Transform CustomerWaitPosition { get; }
        Transform CounterPlacePosition { get; }
        Drink DrinkData { get; }
        bool HasDrinks { get; set; }
    }
}
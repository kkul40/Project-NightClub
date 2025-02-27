using System;
using PropBehaviours;

namespace GameEvents
{
    public static class KEvent_Employee
    {
        public static Action<IBartender> OnBartenderHired;
        public static Action<IBartender> OnBartenderKicked;
        

        public static void TriggerBartenderHired(IBartender bartender)
        {
            OnBartenderHired?.Invoke(bartender);
        }
        public static void TriggerBartenderKicked(IBartender bartender)
        {
            OnBartenderKicked?.Invoke(bartender);
        }
    }
}
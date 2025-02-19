using System;

namespace DefaultNamespace._Refactored.Event
{
    public enum eVerificiationTypes
    {
        Yes,
        No,
        Cancel,
    }
    
    public static class KEvent_UI
    {
        public static Action<Action<eVerificiationTypes>> OnVerificationPopup;

        public static void TriggerVerificationPopup(Action<eVerificiationTypes> function)
        {
            OnVerificationPopup?.Invoke(function);
        }
    }
}
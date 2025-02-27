using System;

namespace GameEvents
{
    public static class KEvent_Cursor
    {
        public static event Action<CursorSystem.eCursorTypes> OnChangeCursor;
        public static event Action OnChangeCursorToPrevious;
        public static event Action OnResetSelection;
        public static event Action OnChangeToDefault;
        

        public static void ChangeCursor(CursorSystem.eCursorTypes newCursor)
        {
            OnChangeCursor?.Invoke(newCursor);
        }

        public static void ChangeToPrevious()
        {
            OnChangeCursorToPrevious?.Invoke();
        }

        public static void ChangeToDefault()
        {
            OnChangeToDefault?.Invoke();
        }

        public static void TriggerResetSelection()
        {
            OnResetSelection?.Invoke();
        }
    }

   
}
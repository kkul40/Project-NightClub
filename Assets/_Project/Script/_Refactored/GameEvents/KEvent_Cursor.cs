using System;
using System.Numerics;
using DiscoSystem;

namespace DefaultNamespace._Refactored.Event
{
    public static class KEvent_Cursor
    {
        public static event Action<CursorSystem.eCursorTypes> OnChangeCursor;
        public static event Action OnChangeCursorToPrevious;
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
    }

   
}
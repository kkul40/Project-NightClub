using System;
using Data;

namespace GameEvents
{
    public static class KEvent_SavingAndLoading
    {
        public static Action<GameData> OnSaved;

        public static void TriggerGameSave(GameData gameData)
        {
            OnSaved?.Invoke(gameData);
        }
    }
}
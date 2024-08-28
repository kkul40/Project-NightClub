using UnityEngine;

namespace System
{
    public class GameTime
    {
        private static float timeScale = 1;
        private static float uiTimeScale = 1;


        public static float GetTimeScale => timeScale;
        public static float DeltaTime => Time.deltaTime * timeScale;


        public static Action OnGamePaused;
        public static Action OnGameResumed;
        

        public static void PauseGame(eTimeType timeType)
        {
            switch (timeType)
            {
                case eTimeType.Game:
                    timeScale = 0f;
                    break;
                case eTimeType.UI:
                    uiTimeScale = 0f;
                    break;
            }
            
            OnGamePaused?.Invoke();
        }

        public static void ResumeGame(eTimeType timeType)
        {
            switch (timeType)
            {
                case eTimeType.Game:
                    timeScale = 1;
                    break;
                case eTimeType.UI:
                    uiTimeScale = 1;
                    break;
            }
            
            OnGameResumed?.Invoke();
        }


    }

    public enum eTimeType
    {
        Game,
        UI,
    }
}
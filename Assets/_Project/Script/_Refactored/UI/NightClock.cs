using System;
using System.Collections;
using UnityEngine;

namespace DiscoSystem
{
    public class NightClock : Singleton<NightClock>
    {
        public int HourMark;
        public int MinuteMark;

        public float TimeSpeed;
        
        public static event Action<int, int> OnClockUpdate;
        public static event Action<int> OnHourPassed;
        public static event Action<int> OnMinutePassed;

        private void Start()
        {
            OnClockUpdate?.Invoke(HourMark, MinuteMark);
            StartCoroutine(ClockCo());
        }

        private IEnumerator ClockCo()
        {
            float secondMark = 0;
            
            while (true)
            {
                secondMark += TimeSpeed * Time.deltaTime;

                if (secondMark >= 60)
                {
                    secondMark = 0;
                    MinuteMark++;

                    if (MinuteMark >= 60)
                    {
                        MinuteMark = 0;
                        HourMark++;

                        if (HourMark == 12)
                        {
                            HourMark = 0;
                        }

                        OnHourPassed?.Invoke(HourMark);
                    }

                    OnMinutePassed?.Invoke(MinuteMark);
                    OnClockUpdate?.Invoke(HourMark, MinuteMark);
                }

                yield return null;
            }
        }
    }
}
using System;
using TMPro;
using UI;
using UnityEngine;

namespace DiscoSystem
{
    public class UIClock : MonoBehaviour
    {
        private TextMeshProUGUI _clock;

        private void Awake()
        {
            _clock = GetComponentInChildren<TextMeshProUGUI>();

            NightClock.OnClockUpdate += UpdateClock;
        }

        private void UpdateClock(int hour, int minute)
        {
            int digitCount = (hour == 0) ? 1 : (int)Math.Floor(Math.Log10(Math.Abs(hour)) + 1);
            string clockText = "";

            if (digitCount > 1)
                clockText += $"{hour} : ";
            else
                clockText += $"0{hour} : ";
            
            digitCount = (minute == 0) ? 1 : (int)Math.Floor(Math.Log10(Math.Abs(minute)) + 1);

            if (digitCount > 1)
                clockText += $"{minute}";
            else
                clockText += $"0{minute}";

            _clock.text = clockText;
        }

        private void OnDestroy()
        {
            NightClock.OnClockUpdate -= UpdateClock;
        }
    }
}
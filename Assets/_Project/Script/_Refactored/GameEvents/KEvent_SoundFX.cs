using System;
using DiscoSystem;

namespace GameEvents
{
    public static class KEvent_SoundFX
    {
        public static Action<SoundFXType, bool> OnSoundFXPlayed;

        public static void TriggerSoundFXPlay(SoundFXType soundFXType, bool delay = false)
        {
            OnSoundFXPlayed?.Invoke(soundFXType, delay);
        }
    }
}
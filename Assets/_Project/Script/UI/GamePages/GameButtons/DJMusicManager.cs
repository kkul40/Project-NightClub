using System;
using Data;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class DJMusicManager : Singleton<DJMusicManager>
    {
        [SerializeField] private SongDataSo _songDataSo;
        private int index = 0;

        private bool isPlaying;

        protected void Start()
        {
            // var djs = DiscoData.Instance.placementDataHandler.GetPropsByType<DJ>();
            // if (djs.Count > 0)
            // {
            //     StartSong(djs[0]);
            // }
        }

        private void Awake()
        {
            GameEvent.Subscribe<Event_PropPlaced>( handle=> StartSong(handle.PropUnit));
            GameEvent.Subscribe<Event_PropRemoved>(handle=> StopSong(handle.PropUnit));
        }

        private void StartSong(IPropUnit propUnit)
        {
            if (propUnit is not DJ) return;

            if (isPlaying) return;
            
            if(_songDataSo.Temp.Count > 0)
                MusicPlayer.Instance.ChangeMusic(_songDataSo.Temp[index].Clip);

            isPlaying = true;
        }

        private void StopSong(IPropUnit propUnit)
        {
            if (propUnit is not DJ) return;
            
            var djs = DiscoData.Instance.GetPlacedPropsByType<DJ>();
            
            Debug.Log(djs.Count);
            if (djs.Count > 0)
                return;

            MusicPlayer.Instance.PauseMusic();
            isPlaying = false;
        }

        public void PlayeNextSong()
        {
            index++;
            if (index >= _songDataSo.Temp.Count)
                index = 0;
            
            MusicPlayer.Instance.ChangeMusic(_songDataSo.Temp[index].Clip);
        }
    }
}
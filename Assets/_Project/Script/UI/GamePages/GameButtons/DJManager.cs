using System;
using Data;
using DiscoSystem;
using GameEvents;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class DJManager : Singleton<DJManager>
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
            GameEvent.Subscribe<Event_PropPlaced>(StartSong);
            GameEvent.Subscribe<Event_PropRemoved>(StopSong);
        }

        private void StartSong(Event_PropPlaced placedEvent)
        {
            if (placedEvent.PropUnit is not DJ) return;

            if (isPlaying) return;

            if (_songDataSo.Temp.Count > 0)
            {
                GameEvent.Trigger(new Event_PlaySong(_songDataSo.Temp[index].Clip));
            }

            isPlaying = true;
        }

        private void StopSong(Event_PropRemoved removedEvent)
        {
            if (removedEvent.PropUnit is not DJ) return;
            
            var djs = DiscoData.Instance.GetPlacedPropsByType<DJ>();
            
            Debug.Log(djs.Count);
            if (djs.Count > 0)
                return;

            // MusicManager.Instance.PauseSong();
            //
            /*
             *
             *
             *
             *
             *
             *
             *
             * 
             */
            
            
            
            
            isPlaying = false;
        }

        public void PlayeNextSong()
        {
            index++;
            if (index >= _songDataSo.Temp.Count)
                index = 0;
            
            GameEvent.Trigger(new Event_PlaySong(_songDataSo.Temp[index].Clip));
        }
    }
}
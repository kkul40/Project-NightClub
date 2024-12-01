using System;
using ScriptableObjects;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class DJMusicManager : Singleton<DJMusicManager>
    {
        [SerializeField] private SongDataSo _songDataSo;
        private int index = 0;

        protected void Start()
        {
            if(_songDataSo.Temp.Count > 0)
                MusicPlayer.Instance.ChangeMusic(_songDataSo.Temp[0].Clip);
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
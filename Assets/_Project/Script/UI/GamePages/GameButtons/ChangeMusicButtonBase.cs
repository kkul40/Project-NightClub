using System;
using ScriptableObjects;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class ChangeMusicButtonBase : UIButtonBase
    {
        [SerializeField] private SongDataSo _songDataSo;
        private int index = 0;

        protected override void Start()
        {
            if(_songDataSo.Temp.Count > 0)
                MusicPlayer.Instance.ChangeMusic(_songDataSo.Temp[0]);
        }

        public override void OnHover()
        {
        }

        public override void OnClick()
        {
            index++;
            if (index >= _songDataSo.Temp.Count)
                index = 0;
            
            MusicPlayer.Instance.ChangeMusic(_songDataSo.Temp[index]);
        }
    }
}
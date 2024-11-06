using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class ChangeMusicButtonBase : UIButtonBase
    {
        public List<AudioClip> Musics;
        private int index = 0;

        protected override void Start()
        {
            if(Musics.Count > 0)
                MusicPlayer.Instance.ChangeMusic(Musics[0]);
        }

        public override void OnHover()
        {
        }

        public override void OnClick()
        {
            index++;
            if (index >= Musics.Count)
                index = 0;
            
            MusicPlayer.Instance.ChangeMusic(Musics[index]);
        }
    }
}
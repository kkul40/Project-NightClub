using UI.GamePages.GameButtons;
using UnityEngine;

namespace ScriptableObjects
{
    public class Candidate_Bartender : UIButtonBase
    {
        public Sprite profile;
        public string bartenderName;
        public int speed;
        public int tipBonus;

        protected override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void OnHover()
        {
            throw new System.NotImplementedException();
        }

        public override void OnClick()
        {
            // Do HIRE
        }
    }
}
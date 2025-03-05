using System;
using System.Character.NPC;
using DiscoSystem;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class UIHireButtion : MonoBehaviour, IButton
    {
        public void OnHover()
        {
        }

        public void OnClick()
        {
            NPCSystem.Instance.CreateCharacter(ePersonType.Bartender);
        }
    }
}
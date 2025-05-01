using System;
using DiscoSystem;
using DiscoSystem.Character;
using DiscoSystem.Character.NPC;
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
using System;
using _Initializer;
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
            ServiceLocator.Get<NPCSystem>().CreateCharacter(ePersonType.Bartender);
        }
    }
}
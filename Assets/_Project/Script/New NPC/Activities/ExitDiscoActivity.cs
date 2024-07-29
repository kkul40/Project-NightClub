using System;
using Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace New_NPC.Activities
{
    public class ExitDiscoActivity : IActivity
    {
        public bool IsEnded { get; private set; }

        private float timer = 0;
        private float delay = 1;

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
            and.Npc.PathFinder.GoTargetDestination(DiscoData.Instance.MapData.EnterencePosition -
                                                   new Vector3(0, 0.5f, 0));
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (and.Npc.PathFinder.HasReachedDestination)
            {
                and.Npc.PathFinder.CancelDestination();
                and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
                timer += Time.deltaTime;
                if (timer > delay) IsEnded = true;
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            Object.Destroy(and.Npc.gameObject);
        }
    }
}
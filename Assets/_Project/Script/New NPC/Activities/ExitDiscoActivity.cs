using System;
using Data;
using UnityEngine;

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
            and.Npc.SetAnimation(eNpcAnimation.Walk);
            and.Npc.pathFinder.TryGoTargetDestination(DiscoData.Instance.MapData.EnterencePosition - new Vector3(0, 0.5f, 0));
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (and.Npc.pathFinder.hasReachedDestination)
            {
                and.Npc.pathFinder.CancelDestination();
                and.Npc.SetAnimation(eNpcAnimation.Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    IsEnded = true;
                }
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            MonoBehaviour.Destroy(and.Npc.gameObject);
        }
    }
}
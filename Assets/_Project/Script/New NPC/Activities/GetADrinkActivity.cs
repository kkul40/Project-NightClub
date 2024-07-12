using System.Collections;
using Data;
using DG.Tweening;
using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class GetADrinkActivity : IActivity
    {
        public bool IsEnded { get; private set; }

        private Bar _bar;
        public void StartActivity(ActivityNeedsData and)
        {
            _bar = and.GetAvaliablePropByType<Bar>(ePlacementLayer.Floor);

            if (_bar == null || !_bar.HasDrinks)
            {
                IsEnded = true;
                return;
            }
            
            and.Npc.pathFinder.GoToDestination(_bar.WaitPosition.position);
            and.Npc.SetAnimation(eNpcAnimation.Walk);

            DOTween.instance.StartCoroutine(CoGetDrink(and));
        }

        public void UpdateActivity(ActivityNeedsData and)
        {
            
        }

        public void EndActivity(ActivityNeedsData and)
        {
            and.Npc.pathFinder.CancelDestination();
            and.Npc.SetAnimation(eNpcAnimation.Idle);
        }

        private IEnumerator CoGetDrink(ActivityNeedsData and)
        {
            while (!and.Npc.pathFinder.hasReachedDestination)
            {
                if (!_bar.HasDrinks)
                {
                    IsEnded = true;
                    yield break;
                }

                yield return null;
            }
            
            yield return new WaitUntil(() => and.Npc.pathFinder.hasReachedDestination);
            
            and.Npc.SetAnimation(eNpcAnimation.Idle);
            and.Npc.pathFinder.SetRotation(_bar.WaitPosition.rotation);
            yield return new WaitForSeconds(1);
            _bar.GetDrink();
            yield return new WaitForSeconds(0.5f);

            WalkRandomActivity temp = new WalkRandomActivity();
            
            and.Npc.pathFinder.GoToDestination(temp.GetRandomDestination(and));
            and.Npc.SetAnimation(eNpcAnimation.Walk);
            
            yield return new WaitUntil(() => and.Npc.pathFinder.hasReachedDestination);
            IsEnded = true;

            yield return null;
        }
    }
}
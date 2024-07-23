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

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            var _bars = and.GetAvaliablePropsByType<Bar>(ePlacementLayer.FloorProp);

            _bar = _bars[Random.Range(0, _bars.Count)];

            if (_bar == null || !_bar.HasDrinks)
            {
                IsEnded = true;
                return;
            }

            if(!and.Npc.pathFinder.TryGoTargetDestination(_bar.WaitPosition.position))
            {
                IsEnded = true;
                return;
            }
            
            and.Npc.SetAnimation(eNpcAnimation.Walk);
            DOTween.instance.StartCoroutine(CoGetDrink(and));
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
        }

        public void OnActivityEnd(ActivityNeedsData and)
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

            IsEnded = true;

            yield return null;
        }
    }
}
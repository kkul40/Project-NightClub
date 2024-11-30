using System.Collections;
using DG.Tweening;
using PropBehaviours;
using UI.Emotes;
using UnityEngine;

namespace NPCBehaviour.Activities
{
    public class GetADrinkActivity : IActivity
    {
        //TODO Yeni BarSistemine Uyarla
        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        private IBar _bar;
        private Coroutine _routine;
        private bool gotADrink = false;
        
        
        public bool CanStartActivity(ActivityNeedsData and)
        {
            var _bars = and.GetAvaliablePropsByInterface<IBar>();
            if (_bars == null) return false;

            _bar = _bars[Random.Range(0, _bars.Count)];
            int iteration = 0;
            while (!_bar.HasDrinks)
            {
                _bar = _bars[Random.Range(0, _bars.Count)];

                if (Helper.IterateTo100(ref iteration)) return false;
            }
 
            return and.Npc.PathFinder.CheckIfPathAvaliable(_bar.CustomerWaitPosition.position);
        }

        public bool OnActivityErrorHandler(ActivityNeedsData and)
        {
            if (_bar == null) return true;
            return false;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.PathFinder.GoTargetDestination(_bar.CustomerWaitPosition.position);
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            _routine = DOTween.instance.StartCoroutine(CoGetDrink(and));
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.PathFinder.CancelDestination();
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            
            if (_routine != null)
            {
                DOTween.instance.StopCoroutine(_routine);
            }
        }

        private IEnumerator CoGetDrink(ActivityNeedsData and)
        {
            yield return new WaitUntil(() => and.Npc.PathFinder.HasReachedDestination);

            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            and.Npc.PathFinder.SetPositioning(_bar.CustomerWaitPosition.rotation);
            yield return new WaitForSeconds(1);
            if (_bar.HasDrinks)
            {
                _bar.GetDrink();
                UIEmoteManager.Instance.ShowEmote(EmoteTypes.Happy, and.Npc);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                UIEmoteManager.Instance.ShowEmote(EmoteTypes.Sad, and.Npc);
            }


            IsEnded = true;

            yield return null;
        }
    }
}
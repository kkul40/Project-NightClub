using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NPCBehaviour;
using NPCBehaviour.Activities;
using UnityEngine;

namespace System
{
    [DisallowMultipleComponent]
    public class ActivitySystem : Singleton<ActivitySystem>
    {
        public Dictionary<Type, IActivity> dictionary = new();

        public override void Initialize(GameInitializer gameInitializer)
        {
            var assembly = Assembly.GetAssembly(typeof(IActivity));
            var allActivityTypes = assembly.GetTypes()
                .Where(t => typeof(IActivity).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var ac in allActivityTypes)
            {
                var activity = Activator.CreateInstance(ac) as IActivity;

                if (activity is NoneActivity) continue;
                if (activity is WalkToEnteranceActivity) continue;
                if (activity is ExitDiscoActivity) continue;
                // if (activity is LeanOnWall) continue;

                dictionary.Add(ac, activity);
            }
        }

        public IActivity GetActivity(Type activity2)
        {
            return Activator.CreateInstance(activity2) as IActivity;
        }

        public IActivity GetRandomActivity()
        {
            var a = UnityEngine.Random.Range(0, dictionary.Count);
            var randomActivity = dictionary.ElementAt(a).Value;
            return GetActivity(dictionary.ElementAt(a).Key);
        }
    }
}
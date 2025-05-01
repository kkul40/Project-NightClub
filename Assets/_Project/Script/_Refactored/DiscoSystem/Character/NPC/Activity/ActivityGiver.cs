using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiscoSystem.Character.NPC.Activity.Activities;
using Random = UnityEngine.Random;

namespace DiscoSystem.Character.NPC.Activity
{
    public class ActivityGiver
    {
        public Dictionary<Type, IActivity> dictionary = new();

        public ActivityGiver()
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
                if (activity is ErrorHandlerActivity) continue;

                dictionary.Add(ac, activity);
            }
        }

        public IActivity GetActivity(Type activity2)
        {
            return Activator.CreateInstance(activity2) as IActivity;
        }

        public IActivity GetRandomActivity()
        {
            var a = Random.Range(0, dictionary.Count);
            var randomActivity = dictionary.ElementAt(a).Value;
            return GetActivity(dictionary.ElementAt(a).Key);
        }
    }
}
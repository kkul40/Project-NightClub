using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NPC.Activities;
using UnityEngine;

namespace System
{
    [DisallowMultipleComponent]
    public class ActivitySystem : Singleton<ActivitySystem>
    {
        public Dictionary<Type, Activity> dictionary = new();

        private void Awake()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            var assembly = Assembly.GetAssembly(typeof(Activity));
            var allActivityTypes = assembly.GetTypes()
                .Where(t => typeof(Activity).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var ac in allActivityTypes)
            {
                var activity = Activator.CreateInstance(ac) as Activity;
                dictionary.Add(ac, activity);
            }
        }

        public Activity GetActivity(Type activity2)
        {
            return Activator.CreateInstance(activity2) as Activity;
        }

        public Activity GetRandomActivity()
        {
            var a = UnityEngine.Random.Range(0, dictionary.Count);
            var randomActivity = dictionary.ElementAt(a).Value;
            return GetActivity(dictionary.ElementAt(a).Key);
        }
    }
}
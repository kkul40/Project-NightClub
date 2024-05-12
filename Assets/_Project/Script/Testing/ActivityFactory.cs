using System;
using UnityEngine;

namespace Testing
{
    // [Serializable]
    // public abstract class Activity2
    // {
    //     
    // }
    // [Serializable]
    // public class Walk : Activity2
    // {
    //     public int a;
    // }
    // [Serializable]
    // public class Idle : Activity2
    // {
    //     public int b;
    // }
    //
    // [Serializable]
    // public class Run : Activity2
    // {
    //     public int c;
    // }
    //
    // public class ActivityFactory : MonoBehaviour
    // {
    //     public Dictionary<Type, Activity2> dictionary = new ();
    //     
    //     private void Awake()
    //     {
    //         var assembly = Assembly.GetAssembly(typeof(Activity2));
    //         var allActivityTypes = assembly.GetTypes().Where(t => typeof(Activity2).IsAssignableFrom(t) && t.IsAbstract == false);
    //     
    //         foreach (var ac in allActivityTypes)
    //         {
    //             var activity = Activator.CreateInstance(ac) as Activity2;
    //             dictionary.Add(ac, activity);
    //         }
    //     }
    //     
    //     public Activity2 GetActivity(Type activity2)
    //     {
    //         return Activator.CreateInstance(activity2) as Activity2;
    //     }
    //     
    //     public Activity2 GetRandomActivity()
    //     {
    //         var a = Random.Range(0, dictionary.Count);
    //         var randomActivity = dictionary.ElementAt(a).Value;
    //         return GetActivity(dictionary.ElementAt(a).Key);
    //     }
    // }
}
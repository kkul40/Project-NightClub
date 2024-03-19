using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class ActivitySystem : MonoBehaviour
{
    public static ActivitySystem Instance;
    [SerializeField] private List<Activity> _activities;

    private Activity Dinner = new DinnerActivity();

    private void Awake()
    {
        Instance = this;
        
        // TODO Add All Activitie Here
        _activities = new List<Activity>();
        _activities.Add(Dinner);
    }

    public Activity GetRandomActivity()
    {
        if (_activities.Count <= 0)
        {
            Debug.LogError("No Activities Added To The List");
            return new EmptyActivity();
        }

        Activity tempActivity = _activities[Random.Range(0, _activities.Count - 1)];

        if (tempActivity is DinnerActivity)
        {
            return new DinnerActivity();
        }

        return new EmptyActivity();
    }
}

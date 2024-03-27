using System.Collections.Generic;
using Activities;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivitySystem : Singleton<ActivitySystem>
{
    [SerializeField] private List<Activity> _activities;

    private void Awake()
    {
        // TODO Add All Activitie Here
        _activities = new List<Activity>();
        _activities.Add(new DinnerActivity());
        _activities.Add(new WalkRandomActivity());
    }

    public Activity GetRandomActivity()
    {
        if (_activities.Count <= 0)
        {
            Debug.LogError("No Activities Added To The List");
            return new NoneActivity();
        }

        var activityIndex = Random.Range(0, _activities.Count);
        Activity temp = _activities[activityIndex];

        if (temp is DinnerActivity)
        {
            return new DinnerActivity();
        }
        else if (temp is WalkRandomActivity)
        {
            return new WalkRandomActivity();
        }
        
        
        return new NoneActivity();
    }
}
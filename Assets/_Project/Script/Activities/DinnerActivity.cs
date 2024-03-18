using System;
using UnityEngine;

[Serializable]
public class DinnerActivity : Activity
{
    private enum DinnerState
    {
        None,
        Waiting,
        Eating,
        Drinking,
    }
    
    private Prop prop;
    private Vector3 positionBeforeSit = Vector3.one;
    private DinnerState _dinnerState = DinnerState.None;

    public override bool isEnded { get; protected set; }
    public override bool isCanceled { get; protected set; }

    public override void StartActivity(NPC npc)
    {
        if (isCanceled) return;
        
        Debug.Log("Dinner Activity Start");
        prop = GetClosestPropByType(PropType.Chair, npc);
        
        if (prop == null)
        {
            isCanceled = true;
            npc.ChangeActivitiy(new EmptyActivity());
            return;
        }
        npc.SetNewTarget(prop.GetPropPosition());
    }

    public override void UpdateActivity(NPC npc)
    {
        if (isCanceled) return;
        
        //TODO Use DinnerState here
        Debug.Log("Dinner Activity Update");

        switch (_dinnerState)
        {
            case DinnerState.None:
                var distance = Vector3.Distance(npc.transform.position, prop.GetPropPosition());
                if (distance < 0.74f)
                {
                    //Deactivate Navmesh
                    //Sit
                    positionBeforeSit = npc.transform.position;
                    npc._navMeshAgent.enabled = false;
                    npc.transform.position = prop.GetPropPosition();
                    npc.transform.rotation = prop.GetPropRotation();
                    npc.ChangeState(NpcState.Sit);
                    _dinnerState = DinnerState.Waiting;
                }
                break;
            case DinnerState.Waiting:
                // Waiting logic here
                break;
            case DinnerState.Eating:
                // Eating logic here
                break;
            case DinnerState.Drinking:
                // Drinking logic here
                break;
        }
    }

    public override void EndActivity(NPC npc)
    {
        if (isCanceled) return;
        
        Debug.Log("Dinner Activity End");
        npc.ChangeState(NpcState.Idle);
        npc._navMeshAgent.enabled = true;
        npc.transform.position = positionBeforeSit;
        isEnded = true;
    }
}
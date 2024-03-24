using System;
using UnityEngine;

[Serializable]
public class DinnerActivity : Activity
{
    private enum DinnerState
    {
        None,
        Sitting,
        Eating,
        Drinking,
    }
    
    private Chair chairProp;
    private Vector3 positionBeforeSit = -Vector3.one;
    private DinnerState _dinnerState = DinnerState.None;

    public override bool isEnded { get; protected set; }
    public override bool isCanceled { get; protected set; }

    public override void StartActivity(NPC npc)
    {
        Debug.LogWarning("Dinner Basladi");
        if (isCanceled) return;
        Debug.LogWarning("Dinner Basladi 222");

        chairProp = GetAvaliablePropByType<Chair>(npc);
        
        if (chairProp == null || chairProp.IsOccupied)
        {
            isCanceled = true;
            return;
        }
        
        npc.SetNewTarget(chairProp.GetPropPosition());
        chairProp.IsOccupied = true;
    }

    public override void UpdateActivity(NPC npc)
    {
        Debug.LogWarning("Dinner Updated");
        if (isCanceled) return;
        
        //TODO Use DinnerState here
        switch (_dinnerState)
        {
            case DinnerState.None:
                var distance = Vector3.Distance(npc.transform.position, chairProp.GetPropPosition());
                if (distance < 0.05f)
                {
                    //Deactivate Navmesh
                    //Sit
                    positionBeforeSit = npc.transform.position;
                    npc._navMeshAgent.enabled = false;
                    npc.transform.position = chairProp.GetItOccupied(npc);
                    npc.transform.rotation = chairProp.GetPropRotation();
                    npc.ChangeState(NpcState.Sit);
                    _dinnerState = DinnerState.Sitting;
                }
                break;
            case DinnerState.Sitting:
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
        chairProp.IsOccupied = false;
        isEnded = true;
    }
}
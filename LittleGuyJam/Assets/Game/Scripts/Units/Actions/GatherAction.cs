using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GatherAction : IAction
{
    ResourceUnit unit;
    Resource target;

    bool fromPlayer;

    bool isGathering;

    float timer;

    public void AssignUnit(Unit unit, bool player)
    {
        this.unit =(ResourceUnit)  unit;
        fromPlayer = player;
        isGathering = true;
        timer = 0;
    }

    public void Execute()
    {
        Gather();
    }

    void Gather()
    {
        // Timer
        if (timer < unit.data.CollectionSpeed)
        {
            timer += Time.deltaTime;
            return;
        }
        else
        {
            timer = 0;
        }

        // Gather
        if (unit.CollectedResources < unit.data.MaxResources)
        {
            Debug.Log(unit.name + " Gather +" + target.Value);
            unit.CollectedResources += target.Value;
        }
        else
        {
            unit.CollectedResources = unit.data.MaxResources;
            Debug.Log(unit.name + " Gathered " + unit.CollectedResources);
            isGathering = false;
            unit.nearTarget = false;
            target = null;
            unit.actionQueue.Remove(this);
        }
    }

    public bool CheckCanExecute()
    {
        if (target == null && unit.CollectedResources < unit.data.MaxResources)
        {
            target = unit.FindResource();
            return false;
        }

        if (!unit.nearTarget 
            && unit.CollectedResources < unit.data.MaxResources 
            && unit.actionQueue.Count < 3 
            && unit.actionQueue[unit.actionQueue.Count - 1].GetType() != typeof(MoveAction))
        {
            isGathering = false;

            MoveAction newMA = unit.NewMoveAction(false);
            newMA.Target = target.transform.position;
            unit.NextTarget = newMA.Target;
            newMA.DistanceCap = unit.data.DistanceThreshold;

            unit.actionQueue.Add(this);
            unit.actionQueue.Add(newMA);
            return false;
        }

        if (unit.nearTarget && unit.CollectedResources < unit.data.MaxResources)
        {
            isGathering = true;
        }

        if (!isGathering)
        {
            return false;
        }

        return true;
    }

    public bool FromPlayer
    {
        get { return fromPlayer; }
    }

    public bool IsGathering
    {
        get { return isGathering; }
        set { isGathering = value; }
    }

    public Resource ResourceTarget
    {
        get { return target; }
        set { target = value; }
    }

}

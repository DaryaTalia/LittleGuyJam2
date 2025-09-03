using Unity.VisualScripting;
using UnityEngine;

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
        if (!CheckCanExecute())
        {
            return;
        }

        // Maintain Position
        Gather();
    }

    void Gather()
    {
        // Timer
        if (timer < unit.data.CollectionSpeed)
        {
            timer++;
            return;
        }
        else
        {
            timer = 0;
        }

        // Gather
        if (unit.CollectedResources < unit.data.MaxResources)
        {
            unit.CollectedResources += target.Value;
        }
        else
        {
            unit.CollectedResources = unit.data.MaxResources;
            isGathering = false;
        }
    }

    public bool CheckCanExecute()
    {
        if (!isGathering)
        {
            return false;
        }

        if (target == null)
        {
            target = unit.FindResource();
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

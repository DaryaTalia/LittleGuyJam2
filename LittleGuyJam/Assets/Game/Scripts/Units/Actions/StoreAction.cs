using UnityEngine;

[System.Serializable]
public class StoreAction : IAction
{
    ResourceUnit unit;

    bool fromPlayer;

    bool isStoring;

    float timer;

    public void AssignUnit(Unit unit, bool player)
    {
        this.unit = (ResourceUnit) unit;
        fromPlayer = player;
        isStoring = true;
        unit.nearTarget = false;
        timer = 0;
    }

    public void Execute()
    {
        Store();
    }

    void Store()
    {
        // Timer
        if (timer < unit.data.StorageSpeed)
        {
            timer += Time.deltaTime;
            return;
        }
        else
        {
            timer = 0;
        }

        // Store
        GameManager.instance.data.TotalCollectedResources += unit.CollectedResources;
        GameManager.instance.data.CurrentAvailableResources += unit.CollectedResources;
        //Debug.Log(unit.name + " Stored " + unit.CollectedResources);
        unit.CollectedResources = 0;
        isStoring = false;
        unit.nearTarget = false;
        unit.actionQueue.Remove(this);
    }

    public bool CheckCanExecute()
    {
        if (!unit.nearTarget 
            && unit.CollectedResources == unit.data.MaxResources 
            && unit.actionQueue.Count < 3 
            && unit.actionQueue[unit.actionQueue.Count - 1].GetType() != typeof(MoveAction))
        {
            isStoring = false;

            MoveAction newMA = unit.NewMoveAction(false);
            newMA.Target = GameManager.instance.Storage.transform.position;
            unit.NextTarget = newMA.Target;
            newMA.DistanceCap = unit.data.DistanceThreshold;

            unit.actionQueue.Add(this);
            unit.actionQueue.Add(newMA);
            return false;
        }

        if (unit.nearTarget && unit.CollectedResources == unit.data.MaxResources)
        {
            isStoring = true;
        }

        if (!isStoring)
        {
            return false;
        }

        return true;
    }

    public bool FromPlayer
    {
        get { return fromPlayer; }
    }

    public bool IsStoring
    {
        get { return isStoring; }
        set { isStoring = value; }
    }

}

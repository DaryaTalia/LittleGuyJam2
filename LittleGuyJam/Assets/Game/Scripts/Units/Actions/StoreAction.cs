using UnityEngine;

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
        timer = 0;
    }

    public void Execute()
    {
        if (!CheckCanExecute())
        {
            return;
        }

        Store();
    }

    void Store()
    {
        // Timer
        if (timer < unit.data.StorageSpeed)
        {
            timer++;
            return;
        }
        else
        {
            timer = 0;
        }

        // Store
        GameManager.instance.data.TotalCollectedResources += unit.CollectedResources;
        GameManager.instance.data.CurrentAvailableResources += unit.CollectedResources;
        isStoring = false;
    }

    public bool CheckCanExecute()
    {
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

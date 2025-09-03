using System.Collections;
using UnityEngine;
using static UnitManager;

public class ResourceUnit : Unit
{
    [SerializeField]
    int collectedResources;

    public ResourceUnit(UnitAlignment _a) : base(_a)
    {
        Role = UnitRole.resource;
    }

    public override bool AutonomyBid(string action)
    {
        int bid = Random.Range(0, data.RandomAutonomyMax);

        switch (action)
        {
            case "Gather":
                {
                    if(bid < data.RandomGatherAutonomy)
                    {
                        return true;
                    }
                    return false;
                }

            case "Store":
                {
                    if(bid < data.RandomStoreAutonomy)
                    {
                        return true;
                    }
                    return false;
                }
        }
        return false;
    }

    public Resource FindResource()
    {
        //Debug.Log(name + " finding resource... ");

        if (GameManager.instance.ResourceManager.activeResourcePool.Count > 0)
        {

            int r = Random.Range(0, GameManager.instance.ResourceManager.activeResourcePool.Count);

            //Debug.Log(name + " resource found ");
            return GameManager.instance.ResourceManager.activeResourcePool[r].GetComponent<Resource>();
        }

        //Debug.Log(name + " resource not found ")
        return null;
    }

    public GatherAction NewGatherAction(bool fromPlayer)
    {
        GatherAction action = new GatherAction();

        action.AssignUnit(this, fromPlayer);
        action.ResourceTarget = FindResource();
        nextTarget = action.ResourceTarget.transform.position;

        return action;
    }

    public StoreAction NewStoreAction(bool fromPlayer)
    {
        StoreAction action = new StoreAction();

        action.AssignUnit(this, fromPlayer);
        nextTarget = GameManager.instance.Storage.transform.position;

        return action;
    }


    // Accessors

    public int CollectedResources
    {
        get { return collectedResources; }
        set { collectedResources = value; }
    }
}

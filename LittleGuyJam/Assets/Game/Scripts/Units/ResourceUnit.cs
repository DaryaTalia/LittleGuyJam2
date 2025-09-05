using System.Collections;
using System.Linq;
using UnityEngine;
using static UnitManager;

public class ResourceUnit : Unit
{
    public Animator resourceAnim;

    [SerializeField]
    int collectedResources;

    public ResourceUnit(UnitAlignment _a) : base(_a)
    {
        Role = UnitRole.resource;
        resourceAnim = GetComponentInChildren<Animator>();
    }

    public override bool AutonomyBid(string action)
    {
        if (actionQueue.Last<IAction>().FromPlayer)
        {
            canBid = true;
        }

        if (canBid)
        {
            int bid = Random.Range(0, data.RandomAutonomyMax);

            switch (action)
            {
                case "Gather":
                    {
                        if (bid < data.RandomGatherAutonomy)
                        {
                            return true;
                        }
                        StartCoroutine(AutonomyBidRefresh());
                        return false;
                    }

                case "Store":
                    {
                        if (bid < data.RandomStoreAutonomy)
                        {
                            return true;
                        }
                        StartCoroutine(AutonomyBidRefresh());
                        return false;
                    }
            }
        }
        
        return false;
    }

    public Resource FindResource()
    {
        if (GameManager.instance.ResourceManager.activeResourcePool.Count > 0)
        {
            int r = Random.Range(0, GameManager.instance.ResourceManager.activeResourcePool.Count);

            return GameManager.instance.ResourceManager.activeResourcePool[r].GetComponent<Resource>();
        }

        return null;
    }

    public GatherAction NewGatherAction(bool fromPlayer)
    {
        GatherAction action = new GatherAction();

        action.AssignUnit(this, fromPlayer);

        if (!fromPlayer)
        {
            action.ResourceTarget = FindResource();
            nextTarget = action.ResourceTarget.transform.position;
        }

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

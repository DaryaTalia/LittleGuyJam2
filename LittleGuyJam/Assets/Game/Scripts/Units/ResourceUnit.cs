using System.Collections;
using UnityEngine;

public class ResourceUnit : Unit
{
    [SerializeField]
    bool hasResources;
    [SerializeField]
    bool isGathering;
    [SerializeField]
    bool isStoring;
    [SerializeField]
    int collectedResources;

    [SerializeField]
    Resource resourceTarget;

    public ResourceUnit(UnitAlignment _a) : base(_a)
    {
        Role = UnitRole.resource;
        Reset();
    }

    public void Reset()
    {
        hasResources = false;
        isGathering = false;
        collectedResources = 0;
        resourceTarget = null;
    }

    public void UpdateResourcer()
    {
        if (resourceTarget == null && !HasResources)
        {
            // Automated Find Resource, TODO: Add random chance for player interaction
            FindResource();
        }

        if (HasResources && !isStoring)
        {
            Target = GameManager.instance.Storage.transform.position;
            TargetAssigned = true;
        }

        if (CurrentState == UnitManager.UnitStates.Gather && !isGathering && !coroutineRunning)
        {
            Debug.Log(name + " StartCoroutine 'Gather' ");

            StartCoroutine(Gather());
        }
        
        if (CurrentState == UnitManager.UnitStates.Store && !isStoring && !coroutineRunning)
        {
            Debug.Log(name + " StartCoroutine 'Store' ");

            StartCoroutine(Store());
        }
    }

    public void FindResource()
    {
        Debug.Log(name + " finding resource... ");

        if (GameManager.instance.ResourceManager.activeResourcePool.Count > 0)
        {

            int r = Random.Range(0, GameManager.instance.ResourceManager.activeResourcePool.Count);

            resourceTarget = GameManager.instance.ResourceManager.activeResourcePool[r].GetComponent<Resource>();
            Target = resourceTarget.transform.position;

            TargetAssigned = true;

            Debug.Log(name + " resource found ");
            return;
        }

        Debug.Log(name + " resource not found ");
    }

    public IEnumerator Gather()
    {
        coroutineRunning = true;

        while (collectedResources < data.MaxResources)
        {
            CanMove = false;
            isGathering = true;

            yield return new WaitForSeconds(data.CollectionSpeed);

            collectedResources += resourceTarget.Value;

            Debug.Log(name + " Collected Resources = " + collectedResources);
        }

        collectedResources = data.MaxResources;

        Debug.Log(name + " Max Resources Collected: " + collectedResources);

        resourceTarget = null;
        Target = GameManager.instance.Storage.transform.position;
        TargetAssigned = true;
        CanMove = true;
        isGathering = false;
        hasResources = true;

        coroutineRunning = false;
    }

    public IEnumerator Store()
    {
        coroutineRunning = true;

        CanMove = false;
        isStoring = true;

        yield return new WaitForSeconds(data.StorageSpeed);

        GameManager.instance.data.TotalCollectedResources += collectedResources;
        GameManager.instance.data.CurrentAvailableResources += collectedResources;

        collectedResources = 0;
        isStoring = false;
        CanMove = true;
        hasResources = false;
        TargetAssigned = false;

        coroutineRunning = false;
    }

    // Accessors

    public Resource ResourceTarget
    {
        get { return resourceTarget; }
        set { resourceTarget = value; }
    }

    public bool IsGathering
    {
        get { return isGathering; }
    }

    public bool IsStoring
    {
        get { return IsStoring; }
    }

    public bool HasResources
    {
        get { return hasResources; }
    }
}

using System.Collections;
using UnityEngine;

public class ResourceUnit : Unit
{
    [SerializeField]
    bool hasResources;
    [SerializeField]
    bool isGathering;
    [SerializeField]
    int collectedResources;

    [SerializeField]
    Resource resourceTarget;

    public Resource ResourceTarget
    {
        get { return resourceTarget; }
        set { resourceTarget = value; }
    }

    public ResourceUnit(UnitAlignment _a) : base(_a)
    {
        Role = UnitRole.resource;
        resourceTarget = null;
    }

    public void UpdateResourcer()
    {
        if (CurrentState == UnitManager.UnitStates.Collect)
        {
            StartCoroutine(Gather());
        }
        else if (CurrentState == UnitManager.UnitStates.Store)
        {
            StartCoroutine(Store());
        }
        else if (resourceTarget != null) { 
            TargetAssigned = true;
        }
        else if (resourceTarget == null) { 
            TargetAssigned = false;
        }
    }

    public IEnumerator Gather()
    {
        if (collectedResources >= data.MaxResources) { 
            collectedResources = data.MaxResources;
            isGathering = false;
            yield break;
        }

        if(Vector3.Distance(gameObject.transform.position, resourceTarget.gameObject.transform.position) > DistanceThreshold)
        {
            Target = resourceTarget.gameObject.transform.position;
            //StartCoroutine(Move());
            //CurrentState = UnitManager.UnitStates.Move;

            TargetAssigned = true;
        } 
        else
        {
            while (collectedResources < data.MaxResources) {
                CanMove = false;
                isGathering = true;
                yield return new WaitForSeconds(data.CollectionSpeed);
                collectedResources++;
            }
            CanMove = true;

            isGathering = false;

            TargetAssigned = false;
        }
    }

    public IEnumerator Store()
    {
        if (collectedResources <= 0)
        {
            hasResources = false;
            yield break;
        }

        if (Vector3.Distance(gameObject.transform.position, GameManager.instance.Storage.transform.position) > DistanceThreshold)
        {
            Target = GameManager.instance.Storage.transform.position;
            //StartCoroutine(Move());
            //CurrentState = UnitManager.UnitStates.Move;

            TargetAssigned = true;
        }
        else
        {
            CanMove = false;

            yield return new WaitForSeconds(data.StorageSpeed);

            GameManager.instance.data.TotalCollectedResources += collectedResources;
            GameManager.instance.data.CurrentAvailableResources += collectedResources;

            collectedResources = 0;

            CanMove = true;
            TargetAssigned = false;
        }
    }

    public bool IsGathering
    {
        get { return isGathering; }
    }

    public bool HasResources
    {
        get { return hasResources; }
    }
}

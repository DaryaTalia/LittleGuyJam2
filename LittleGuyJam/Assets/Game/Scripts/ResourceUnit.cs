using System.Collections;
using UnityEngine;

public class ResourceUnit : Unit
{
    [SerializeField]
    bool isGathering;
    [SerializeField]
    int collectedResources;

    Resource resourceTarget;

    public ResourceUnit(UnitAlignment _a) : base(_a)
    {
        //StartCoroutine(Move(Vector3.zero));
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
            StartCoroutine(Move(resourceTarget.gameObject.transform.position));
        } else
        {
            while (collectedResources < data.MaxResources) { 
                isGathering = true;
                yield return new WaitForSeconds(data.CollectionSpeed);
                collectedResources++;
            }

            isGathering = false;
        }
    }

    public bool IsGathering
    {
        get { return isGathering; }
    }
}

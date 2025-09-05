using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Resource : MonoBehaviour
{
    public ResourceData data;

    bool collected;

    public bool Collected
    {
        get { return collected; }
        set { collected = value; }
    }

    public int Value
    {
        get { return data.Value; }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.UnitManager.selectedUnits.Count > 0)
        {
            foreach(ResourceUnit r in GameManager.instance.UnitManager.selectedUnits)
            {
                r.NextTarget = transform.position;
                r.actionQueue.Add(r.NewGatherAction(true));
                r.actionQueue.Last<IAction>().ConvertTo<GatherAction>().ResourceTarget = this;
                r.actionQueue.Add(r.NewMoveAction(true));
                r.actionQueue.Last<IAction>().ConvertTo<MoveAction>().Target = r.NextTarget;
                r.actionQueue.Last<IAction>().ConvertTo<MoveAction>().DistanceCap = r.data.DistanceThreshold;
            }

            GameManager.instance.UnitManager.selectedUnits.Clear();
        }
    }

    private void OnMouseOver()
    {
        // Show Info Tooltip
    }
}

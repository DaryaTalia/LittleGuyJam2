using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Resource : GamePiece
{
    public ResourceData data;

    bool collected;

    [SerializeField]
    public GameObject infoUI;

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
                r.NextTarget = this;
                r.actionQueue.Add(r.NewGatherAction(true));
                r.actionQueue.Last<IAction>().ConvertTo<GatherAction>().ResourceTarget = this;
                r.actionQueue.Add(r.NewMoveAction(true));
                r.actionQueue.Last<IAction>().ConvertTo<MoveAction>().Target = this;
                r.actionQueue.Last<IAction>().ConvertTo<MoveAction>().DistanceCap = r.data.DistanceThreshold;
                GameManager.instance.UnitManager.selectedUnits.Remove(r);
                GameManager.instance.UnitManager.selectedUnits.Clear();
                break;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (infoUI != null)
        {
            infoUI.SetActive(false);
        }
    }
}

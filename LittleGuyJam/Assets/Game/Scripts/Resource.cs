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
                r.ResourceTarget = this;
                r.Target = transform.position;
            }
        }
    }
}

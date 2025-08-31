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
}

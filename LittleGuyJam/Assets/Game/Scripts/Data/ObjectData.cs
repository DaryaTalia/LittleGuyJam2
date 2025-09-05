using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public static string id;

    // Transform Data
    public Vector2 position;
    public Vector3 rotation;
    public Vector3 scale;
}

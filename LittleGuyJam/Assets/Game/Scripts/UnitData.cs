using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public float MaxHealth = 100;
    public float MovementSpeed = 10;
    public float DistanceThreshold = 1;
    public int MaxResources = 16;
    public float CollectionSpeed = 10;
    public float AttackRange = 5;
    public float AttackDamage = 10;
    public float AttackSpeed = 5;
}

using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public float MaxHealth = 100;
    public float MovementSpeed = 10;
    public float DistanceThreshold = 1;
    public int MaxResources = 16;
    public float CollectionSpeed = 10;
    public float StorageSpeed = 5;
    public float AttackRange = 5;
    public float AttackDamage = 10;
    public float AttackSpeed = 5;

    [Header("AI Autonomy")]
    [Description("How likely will any individual AI unit spontaneously act in accordance to their role?")]
    public float RandomAutonomyMax = 250;
    public float RandomGatherAutonomy = 75; 
    public float RandomStoreAutonomy = 90; 
    public float RandomAttackAutonomy = 75; 
    public float RandomMoveAttackAutonomy = 90; 
    public float RandomProtectAutonomy = 50; 
}

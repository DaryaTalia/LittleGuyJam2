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
    public int RandomAutonomyMax = 250;
    public int BidRefreshRate = 5;

    [Space]

    public int RandomMoveAutonomy = 75; 
    public int RandomGatherAutonomy = 75; 
    public int RandomStoreAutonomy = 90; 
    public int RandomAttackAutonomy = 75; 
    public int RandomMoveAttackAutonomy = 90; 
    public int RandomProtectAutonomy = 50; 
}

using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public float MaxHealth = 100;
    [Range(0, 5)]
    public float MovementSpeed = 1.5f;
    [Range(0,1)]
    public float DistanceThreshold =.5f;
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

    [Range(0, 250)]
    public int RandomMoveAutonomy = 50;
    [Range(0, 250)]
    public int RandomGatherAutonomy = 25;
    [Range(0, 250)]
    public int RandomStoreAutonomy = 250;
    [Range(0, 250)]
    public int RandomAttackAutonomy = 75;
    [Range(0, 250)]
    public int RandomMoveAttackAutonomy = 90;
    [Range(0, 250)]
    public int RandomProtectAutonomy = 50;
}

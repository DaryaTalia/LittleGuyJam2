using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    public int TotalCollectedResources;
    public int CurrentAvailableResources;

    public int TotalUnits;
    public int TotalBuildings;

    public int TotalGameTime; //in seconds

    public int TotalEnemyUnitsKilled;

    public int WavesPassed;
}

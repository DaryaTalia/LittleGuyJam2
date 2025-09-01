using System.Resources;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(UnitManager))]
[RequireComponent(typeof(ResourceManager))]

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData data;
    UnitManager unitManager;
    ResourceManager resourceManager;
    HUDManager hudManager;

    public enum GameStatus { inactive, playing, paused };
    public GameStatus status;

    [SerializeField]
    GameObject storageBuilding;

    [SerializeField]
    GameObject barracksBuilding;

    [SerializeField]
    GameObject rndBuilding;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        unitManager = GetComponent<UnitManager>();  
        resourceManager = GetComponent<ResourceManager>();

        ResetGameData();
    }

    public void ResetGameData()
    {
        data.TotalCollectedResources = 0;
        data.CurrentAvailableResources = 0;
        data.TotalUnits = 0;
        data.TotalBuildings = 0;
        data.TotalGameTime = 0;
        data.TotalEnemyUnitsKilled = 0;
        data.WavesPassed = 0;
    }

    public UnitManager UnitManager
    {
        get { return unitManager; }
    }
    public ResourceManager ResourceManager
    {
        get { return resourceManager; }
    }

    public void Initialize()
    {
        Initialize();
    }

    public void Update()
    {
        if (status == GameStatus.inactive)
        {

        }

        if (status == GameStatus.playing) { 
            unitManager.UpdateUnits();
        }
    }

    public bool BuyUnit(Building b)
    {
        if(data.CurrentAvailableResources > b.data.ResourceCost)
        {
            data.CurrentAvailableResources -= b.data.ResourceCost;
            SpawnUnit(b.data.UnitTypePrefab);
            return true;
        }
        return false;
    }

    public void SpawnUnit(GameObject u)
    {

    }

    public GameObject Storage
    {
        get { return storageBuilding; }
    }

    public GameObject Barracks
    {
        get { return barracksBuilding; }
    }

    public GameObject RnD
    {
        get { return rndBuilding; }
    }
}

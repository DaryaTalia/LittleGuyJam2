using System.Collections;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(UnitManager))]
[RequireComponent(typeof(ResourceManager))]
[RequireComponent(typeof(HUDManager))]

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData data;

    UnitManager unitManager;
    ResourceManager resourceManager;
    HUDManager hudManager;
    MenuManager menuManager;
    AudioManager audioManager;

    public enum GameMode { timed, elimination, survival };

    public GameMode mode;

    [SerializeField]
    GameObject storageBuilding;

    [SerializeField]
    GameObject barracksBuilding;

    [SerializeField]
    GameObject rndBuilding;

    [SerializeField]
    GameObject map;

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
        hudManager = GetComponent<HUDManager>();
        menuManager = GetComponent<MenuManager>();
        audioManager = GetComponent<AudioManager>();

        audioManager.StartAudio();
        menuManager.StartMenu();
        menuManager.status = MenuManager.MenuStatus.MainMenu;
        UnitManager.StartUnits();
        unitManager.DeactivatePool();
        map.SetActive(false);
    }

    public void Update()
    {
        if (menuManager.status != MenuManager.MenuStatus.Game)
        {
            hudManager.hudPanel.SetActive(false);
            Map.SetActive(false);
        } 
        else
        {
            unitManager.UpdateUnits();
            hudManager.UpdateHUD();
            menuManager.UpdateMenu();

            if (mode == GameMode.timed)
            {
                CheckTimedProgression();
            } 
            else if (mode == GameMode.elimination)
            {
                CheckEliminationProgression();
            }
            else if (mode == GameMode.survival)
            {
                CheckSurvivalProgression();
            }
        }
    }

    public IEnumerator TimerAscending()
    {
        if (menuManager.status == MenuManager.MenuStatus.Game)
        {
            yield return new WaitForSeconds(1);
            data.TotalGameTime++;
        }
        else
        {
            yield return new WaitForSeconds(1);
        }
    }

    public void PlayGame()
    {
        ResetGameData();
        menuManager.CloseGameModePanel();
        hudManager.ResetHUD();
        hudManager.StartHUD();
        hudManager.hudPanel.SetActive(true);
        map.SetActive(true);
        menuManager.status = MenuManager.MenuStatus.Game;
        SpawnUnit(Storage.GetComponent<Building>().data.UnitTypePrefab, Storage.transform.position);
        unitManager.ActivatePool();

        StartCoroutine(TimerAscending());
    }

    public void SelectGameMode(string game)
    {
        switch (game)
        {
            case "Timed":
                {
                    mode = GameMode.timed;
                    hudManager.timeGUIEnabled = true;
                    break;
                }

            case "Elimination":
                {
                    mode = GameMode.elimination;
                    hudManager.enemiesGUIEnabled = true;
                    break;
                }

            case "Survival":
                {
                    mode = GameMode.survival;
                    hudManager.wavesGUIEnabled = true;
                    break;
                }

            default:
                {
                    mode = GameMode.timed;
                    hudManager.timeGUIEnabled = true;
                    break;
                }
        }

        hudManager.resourceGUIEnabled = true;
        hudManager.unitsGUIEnabled = true;
        hudManager.pauseGUIEnabled = true;

        hudManager.hudPanel.SetActive(true);
    }

    public bool BuyUnit(Building b)
    {
        if (data.CurrentAvailableResources > b.data.ResourceCost)
        {
            data.CurrentAvailableResources -= b.data.ResourceCost;
            SpawnUnit(b.data.UnitTypePrefab, b.transform.position);
            return true;
        }
        return false;
    }

    public void SpawnUnit(GameObject u, Vector3 position)
    {
        GameObject newUnit = Instantiate(u, unitManager.activeUnitPool.transform);

        int yOffset, xOffset;

        yOffset = Random.Range(-2,2);
        xOffset = Random.Range(-2,2);

        newUnit.transform.position = new Vector3(
            position.x + xOffset,
            position.y + yOffset, -11);
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

    public void CheckTimedProgression()
    {
        // Activate Buildings
        if(data.TotalCollectedResources >= 64
            && !barracksBuilding.activeInHierarchy)
        {
            barracksBuilding.SetActive(true);
        }
        else if (data.TotalCollectedResources >= 250
            && !rndBuilding.activeInHierarchy)
        {
            rndBuilding.SetActive(true);
        }

        // Spawn Enemies
        if (unitManager.activeUnitPool.
            GetComponentsInChildren<AttackUnit>().
            Where(u => u.Alignment == UnitManager.UnitAlignment.ally).
            Count() > 1)
        {
            int chance = Random.Range(0, 200);

            if (chance < 20)
            {
                if (chance % 2 == 0)
                {
                    SpawnUnit(unitManager.enemyMeleePrefab, unitManager.enemySpawn.position);
                } 
                else
                {
                    SpawnUnit(unitManager.enemyRangedPrefab, unitManager.enemySpawn.position);
                }

            }
        }

    }

    public void CheckEliminationProgression()
    {

    }

    public void CheckSurvivalProgression()
    {

    }

    // Accessors

    public UnitManager UnitManager
    {
        get { return unitManager; }
    }

    public ResourceManager ResourceManager
    {
        get { return resourceManager; }
    }

    public HUDManager HUDManager
    {
        get { return hudManager; }
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

    public GameObject Map
    {
        get { return map; }
    }
}

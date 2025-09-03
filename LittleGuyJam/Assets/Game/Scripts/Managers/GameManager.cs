using System.Collections;
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

    public enum GameStatus { inactive, playing, paused };
    public enum GameMode { timed, elimination, survival };

    public GameStatus status;
    public GameMode mode;

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
        hudManager = GetComponent<HUDManager>();
        menuManager = GetComponent<MenuManager>();
        audioManager = GetComponent<AudioManager>();

        audioManager.StartAudio();
        menuManager.StartMenu();
        status = GameStatus.inactive;
    }

    public void Update()
    {
        if (status == GameStatus.inactive || status == GameStatus.paused)
        {
            hudManager.hudPanel.SetActive(false);
        }

        if (status == GameStatus.playing)
        {
            unitManager.UpdateUnits();
            hudManager.UpdateHUD();
        }
    }

    public IEnumerator TimerAscending()
    {
        while (status == GameStatus.playing || status == GameStatus.paused)
        {
            if (status == GameStatus.playing)
            {
                yield return new WaitForSeconds(1);
                data.TotalGameTime++;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void PlayGame()
    {
        ResetGameData();
        menuManager.CloseGameModePanel();
        hudManager.ResetHUD();
        hudManager.StartHUD();
        hudManager.hudPanel.SetActive(true);
        status = GameStatus.playing;

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
            SpawnUnit(b.data.UnitTypePrefab);
            return true;
        }
        return false;
    }

    public void SpawnUnit(GameObject u)
    {

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
}

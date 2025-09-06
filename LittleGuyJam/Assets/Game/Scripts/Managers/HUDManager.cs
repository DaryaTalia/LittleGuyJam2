using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public GameObject hudPanel;

    [Header("Always Enabled HUD")]
    // Always Enabled
    [SerializeField]
    GameObject resourceGUI;
    [SerializeField]
    TextMeshProUGUI resourceText;
    public bool resourceGUIEnabled;

    [Space]

    [SerializeField]
    GameObject unitCountGUI;
    [SerializeField]
    TextMeshProUGUI unitCountText;
    public bool unitsGUIEnabled;

    [Space]

    [SerializeField]
    GameObject pauseGUI;
    Button pauseButton;
    public bool pauseGUIEnabled;

    [Space]

    // Timed Mode
    [Header("Timed Mode HUD")]
    [SerializeField]
    GameObject timePassedGUI;
    [SerializeField]
    TextMeshProUGUI timePassedText;
    public bool timeGUIEnabled;

    [Space]

    // Elimnation Mode
    [Header("Elimination Mode  HUD")]
    [SerializeField]
    GameObject enemiesKilledGUI;
    [SerializeField]
    TextMeshProUGUI enemiesKilledText;
    public bool enemiesGUIEnabled;

    [Space]

    // Survival Mode
    [Header("Survival Mode HUD")]
    [SerializeField]
    GameObject wavesGUI;
    [SerializeField]
    TextMeshProUGUI wavesText;
    public bool wavesGUIEnabled;

    private void Awake()
    {
        pauseButton = pauseGUI.GetComponent<Button>();

        if(resourceGUI != null)
        {
            resourceGUI.SetActive(false);
        }

        if(unitCountGUI != null)
        {
            unitCountGUI.SetActive(false);
        }

        if(timePassedGUI != null)
        {
            timePassedGUI.SetActive(false);
        }

        if(enemiesKilledGUI != null)
        {
            enemiesKilledGUI.SetActive(false);
        }

        if(wavesGUI != null)
        {
            wavesGUI.SetActive(false);
        }

        if(pauseGUI != null)
        {
            pauseGUI.SetActive(false);
        }
    }

    public void StartHUD()
    {
        if (resourceGUIEnabled)
        {
            resourceGUI.SetActive(true);
        } 
        else
        {
            resourceGUI.SetActive(false);
        }

        if (unitsGUIEnabled)
        {
            unitCountGUI.SetActive(true);
        }
        else
        {
            unitCountGUI.SetActive(false);
        }

        if (timeGUIEnabled)
        {
            timePassedGUI.SetActive(true);  
        }
        else
        {
            timePassedGUI.SetActive(false);
        }

        if (enemiesGUIEnabled)
        {
            enemiesKilledGUI.SetActive(true);
        }
        else
        {
            enemiesKilledGUI.SetActive(false);
        }

        if (wavesGUIEnabled)
        {
            wavesGUI.SetActive(true);
        }
        else
        {
            wavesGUI.SetActive(false);
        }

        if (pauseGUIEnabled)
        {
            pauseGUI.SetActive(true);
        }
        else
        {
            pauseGUI.SetActive(false);
        }
    }

    public void ResetHUD()
    {
        if (resourceGUIEnabled)
        {
            resourceText.text = "";
        }

        if (unitsGUIEnabled)
        {
            unitCountText.text = "";
        }

        if (timeGUIEnabled)
        {
            timePassedText.text = "";
        }

        if (enemiesGUIEnabled)
        {
            enemiesKilledText.text = "";
        }

        if (wavesGUIEnabled)
        {
            wavesText.text = "";
        }
    }

    public void UpdateHUD()
    {
        if (resourceGUIEnabled)
        {
            resourceText.text = GameManager.instance.data.CurrentAvailableResources.ToString();
        }
        
        if (unitsGUIEnabled)
        {
            unitCountText.text = GameManager.instance.data.TotalUnits.ToString();
        }
        
        if (timeGUIEnabled)
        {
            // Time counting up, not time counting down
            System.TimeSpan t = System.TimeSpan.FromSeconds(GameManager.instance.data.TotalGameTime);

            string time = "";

            if (t.Seconds < 10)
            {
                time = string.Format("{0:D1}",
                                t.Seconds);
            } 
            else if(t.Seconds < 60)
            {
                time = string.Format("{0:D2}",
                t.Seconds);
            }
            else if(t.Minutes < 10)
            {
                time = string.Format("{0:D1}:{1:D2}",
                t.Minutes,
                t.Seconds);
            }
            else
            {
                time = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds);
            }

            timePassedText.text = time;
        }
        
        if (enemiesGUIEnabled)
        {
            enemiesKilledText.text = GameManager.instance.data.TotalEnemyUnitsKilled.ToString();
        }
        
        if (wavesGUIEnabled)
        {
            wavesText.text = GameManager.instance.data.WavesPassed.ToString();
        }
    }


}

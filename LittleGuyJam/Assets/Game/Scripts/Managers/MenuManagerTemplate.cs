using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class MenuManager : MonoBehaviour
{
    public enum MenuStatus { Game, MainMenu, GameModeSelection, Pause, Settings, Credits };
    public MenuStatus status;
    public MenuStatus lastStatus;

    public static MenuManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void StartMenu()
    {
        status = MenuStatus.MainMenu;   
        _mainMenuPanel.SetActive(true);
    }

    public void UpdateMenu()
    {
        if (status == MenuStatus.Game && Input.GetKey(KeyCode.P))
        {
            PauseGame();
        }        
        else if (status != MenuStatus.MainMenu && Input.GetKey(KeyCode.P))
        {
            ResumeGame();
        }
    }

    #region Main Menu

    [Header("Main Menu")]
    [SerializeField]
    GameObject _mainMenuPanel;

    [SerializeField]
    Button _playGameButton;
    [SerializeField]
    Button _settingsButton;
    [SerializeField]
    Button _creditsButton;
    [SerializeField]
    Button _quitGameButton;

    public void PlayGame()
    {
        OpenGameModePanel();

        // Load data or start new data file

        // If data exists, update HUD and DD location
    }

    public void QuitGame() { 
        // Save data from DD_Data to file

        Application.Quit();
    }

    #endregion

    [Header("Game Mode Selection")]
    [SerializeField]
    GameObject _gameModePanel;

    public void OpenGameModePanel()
    {
        lastStatus = status;
        status = MenuStatus.GameModeSelection;
        _mainMenuPanel.SetActive(false);
        _gameModePanel.SetActive(true);
    }

    public void CloseGameModePanel()
    {
        lastStatus = status;
        status = MenuStatus.Game;
        _gameModePanel.SetActive(false);
    }


    #region Settings

    [Header("Settings")]

    [SerializeField]
    GameObject _settingsPanel;

    public void OpenSettings()
    {
        if (status == MenuStatus.MainMenu || status == MenuStatus.Pause)
        {
            lastStatus = status;
            _settingsPanel.SetActive(true);
            status = MenuStatus.Settings;
        }
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
        status = lastStatus;
        lastStatus = MenuStatus.Settings;
    }


    #endregion

    #region Credits

    [Header("Credits")]
    [SerializeField]
    GameObject _creditsPanel;

    public void OpenCredits()
    {
        if (status == MenuStatus.MainMenu || status == MenuStatus.Pause)
        {
            lastStatus = status;
            _creditsPanel.SetActive(true);
            status = MenuStatus.Credits;
        }
    }

    public void CloseCredits()
    {
        _creditsPanel.SetActive(false);
        status = lastStatus;
        lastStatus = MenuStatus.Credits;
    }

    #endregion

    #region Pause Menu

    [Header("Pause Menu")]
    [SerializeField]
    GameObject _pauseMenuPanel;

    [SerializeField]
    Button _resumeGameButton;
    [SerializeField]
    Button _settingsButtonPause;
    [SerializeField]
    Button _creditsButtonPause;
    [SerializeField]
    Button _quitGameButtonPause;

    public void PauseGame()
    {
        if (status != MenuStatus.Pause || 
            status != MenuStatus.GameModeSelection || 
            status != MenuStatus.MainMenu)
        {
            lastStatus = status;
            status = MenuStatus.Pause;
            _pauseMenuPanel.SetActive(true);
            GameManager.instance.Map.SetActive(false);

            Time.timeScale = 0;
        } 
    }

    public void ResumeGame()
    {
        if (status != MenuStatus.Game ||
            status != MenuStatus.GameModeSelection ||
            status != MenuStatus.MainMenu)
        {
            lastStatus = status;
            status = MenuStatus.Game;
            _pauseMenuPanel.SetActive(false);
            CloseCredits();
            CloseSettings();
            GameManager.instance.Map.SetActive(true);

            Time.timeScale = 1;
        }
    }

    #endregion

}

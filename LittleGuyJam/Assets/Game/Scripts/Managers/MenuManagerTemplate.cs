using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class MenuManager : MonoBehaviour
{
    public enum MenuStatus { Game, MainMenu, GameModeSelection, Pause, Settings, Credits };
    public MenuStatus status;

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

    private void Start()
    {
        status = MenuStatus.MainMenu;        
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
        status = MenuStatus.Game;
        _mainMenuPanel.SetActive(false);
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
        _gameModePanel.SetActive(true);
    }

    public void CloseGameModePanel()
    {
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
            _settingsPanel.SetActive(true);
            status = MenuStatus.Settings;
        }
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);

        if (status == MenuStatus.Pause)
        {
            _pauseMenuPanel.SetActive(true);
        } 
        else if (status == MenuStatus.MainMenu)
        {
            _mainMenuPanel.SetActive(true);
        }
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
            _creditsPanel.SetActive(true);
            status = MenuStatus.Credits;
        }
    }

    public void CloseCredits()
    {
        _creditsPanel.SetActive(false);
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
        if (status != MenuStatus.Pause || status != MenuStatus.MainMenu)
        {
            status = MenuStatus.Pause;
            _pauseMenuPanel.SetActive(true);  
            
            Time.timeScale = 0;
        } 
    }

    public void ResumeGame()
    {
        if (status != MenuStatus.MainMenu)
        {            
            status = MenuStatus.Game;
            _pauseMenuPanel.SetActive(false);

            Time.timeScale = 1;
        }
    }

    #endregion

}

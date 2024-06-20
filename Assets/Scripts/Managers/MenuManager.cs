using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuName
{
    None,
    Pause,
    Inventory,
    Settings,
    Debug
}

public class MenuManager : Singleton<MenuManager>
{
    private PlayerPauseController _playerPauseController;

    [SerializeField] public GameObject PauseLevelMenu;
    [SerializeField] public Selectable PauseLevelMenuDefaultSelectable;
    [SerializeField] public GameObject PauseLevelMenuDebug;
    [SerializeField] public Selectable PauseLevelMenuDebugDefaultSelectable;
    [SerializeField] public GameObject PauseHubMenu;
    [SerializeField] public Selectable PauseHubMenuDefaultSelectable;
    [SerializeField] public GameObject PauseHubMenuDebug;
    [SerializeField] public Selectable PauseHubMenuDebugDefaultSelectable;
    private GameObject _pauseMenu;
    private Selectable _pauseMenuDefaultSelectable;
    [SerializeField] public GameObject InventoryMenu;
    [SerializeField] public Selectable InventoryMenuDefaultSelectable;
    [SerializeField] public GameObject SettingsMenu;
    [SerializeField] public Selectable SettingsMenuDefaultSelectable;
    [SerializeField] public GameObject DebugMenu;
    [SerializeField] public Selectable DebugMenuDefaultSelectable;

    private MenuName _currentMenu;

    private void Start()
    {
        _playerPauseController = DataManager.Instance.PlayerStatusObject.Player.GetComponent<PlayerPauseController>();

        //set pause menu for level
        if (GameManager.Instance.IsHubLevel && !GameManager.Instance.HasDebugMenu)
        {
            _pauseMenu = PauseHubMenu;
            _pauseMenuDefaultSelectable = PauseHubMenuDefaultSelectable;
        }
        else if (!GameManager.Instance.HasDebugMenu)
        {
            _pauseMenu = PauseLevelMenu;
            _pauseMenuDefaultSelectable = PauseLevelMenuDefaultSelectable;
        }
        else if (GameManager.Instance.IsHubLevel)
        {
            _pauseMenu = PauseHubMenuDebug;
            _pauseMenuDefaultSelectable = PauseHubMenuDebugDefaultSelectable;
        }
        else
        {
            _pauseMenu = PauseLevelMenuDebug;
            _pauseMenuDefaultSelectable = PauseLevelMenuDebugDefaultSelectable;
        }

        //hide menus
        HideMenu();
    }

    public void Resume()
    {
        _playerPauseController.OnResume();
    }

    public void RestartLevel()
    {
        _playerPauseController.OnResume();
        EventManager.Instance.OnRestartLevel.TriggerEvent(Vector3.zero);
    }

    public void ExitLevel()
    {
        _playerPauseController.OnResume();
        EventManager.Instance.OnExitLevel.TriggerEvent(Vector3.zero);
    }

    public void QuitGame()
    {
        _playerPauseController.OnResume();
        EventManager.Instance.OnQuitGame.TriggerEvent(Vector3.zero);
    }

    public void HideMenu()
    {
        _pauseMenu.SetActive(false);
        InventoryMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        DebugMenu.SetActive(false);

        _currentMenu = MenuName.None;
    }

    public void ShowPauseMenu()
    {
        HideMenu();
        _pauseMenu.SetActive(true);
        _pauseMenuDefaultSelectable.Select();

        _currentMenu = MenuName.Pause;
    }

    public void ShowInventoryMenu()
    {
        HideMenu();
        InventoryMenu.SetActive(true);
        InventoryMenuDefaultSelectable.Select();

        _currentMenu = MenuName.Inventory;
    }

    public void ShowSettingsMenu()
    {
        HideMenu();
        SettingsMenu.SetActive(true);
        SettingsMenuDefaultSelectable.Select();

        _currentMenu = MenuName.Settings;
    }

    public void ShowDebugMenu()
    {
        if (GameManager.Instance.HasDebugMenu)
        {
            HideMenu();
            DebugMenu.SetActive(true);
            DebugMenuDefaultSelectable.Select();

            _currentMenu = MenuName.Debug;
        }
    }

    public MenuName GetCurrentMenu()
    {
        return _currentMenu;
    }
}

using _Project._Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : Menu
{
    [SerializeField] private GameObject     pauseMenuUI;
    [SerializeField] private Button         resumeButton;
    [SerializeField] private Button         settingsButton;
    [SerializeField] private Button         quitButton;

    public override void SetupMenu()
    {
        //InputManager.OpenPauseMenu += ToggleMenu;
        resumeButton.onClick.AddListener(CloseMenu);
        settingsButton.onClick.AddListener(() => Debug.Log("Settings"));
        quitButton.onClick.AddListener(() => Debug.Log("Quit"));
    }

    /*
    public override void ToggleMenu()
    {
        _isPaused = !_isPaused;
        pauseMenuUI.SetActive(_isPaused);
        if (_isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    uiManager.ToggleMenu(this);
    */
    
    public override void ToggleMenu()
    {
        if (gameObject.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            gameObject.SetActive(true);
            UIManager.Instance.SetMenuOpened(this);
        }
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        UIManager.Instance.SetMenuClosed(this);
    }
    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(CloseMenu);
        settingsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }
}
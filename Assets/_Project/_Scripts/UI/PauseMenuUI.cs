using _Project._Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : Menu
{
    [SerializeField] private GameObject     pauseMenuUI;
    [SerializeField] private Button         resumeButton;
    [SerializeField] private Button         settingsButton;
    [SerializeField] private Button         quitButton;
    
    private bool _isPaused;

    public override void SetupMenu()
    {
        //InputManager.OpenPauseMenu += ToggleMenu;
        resumeButton.onClick.AddListener(ToggleMenu);
        settingsButton.onClick.AddListener(() => Debug.Log("Settings"));
        quitButton.onClick.AddListener(() => Debug.Log("Quit"));
    }

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
    
    
    private void OnDestroy()
    {
        //InputManager.OpenPauseMenu -= ToggleMenu;
    }
}